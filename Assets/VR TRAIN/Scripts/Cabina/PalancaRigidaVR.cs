using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

// Este script NO mueve el pomo (eso lo hace FiltroPalancaVR, el Grab Transformer de XRI, que
// corre en todas las fases del pipeline de interaccion incluida "Before Render"). Aqui solo
// LEEMOS la X resultante para detectar la marcha mas cercana, y aplicamos el snap exacto al soltar.
public class PalancaRigidaVR : MonoBehaviour
{
    [Header("Referencias de Jerarquia")]
    public Transform basePalanca;                   // Objeto 'Base' (Pivote local orientable)
    public Transform pomoPalanca;                   // Objeto 'Pomo_Palanca'
    public XRGrabInteractable interactable;         // XR Grab Interactable del Pomo
    public ControladorTrenVR controladorTren;     // Script del Tren

    [Header("Posiciones Exactas de Snap en X Local")]
    public float xMinima = -0.0654f;                // Fase 0 (Abajo / Detenido)
    public float xMedia = -0.0006f;                // Fase 1 (Medio / Vel. Media)
    public float xMaxima = 0.0693f;                // Fase 2 (Arriba / Vel. Maxima)

    private float yFija;
    private float zFija;
    private int marchaActual = 0;
    private IXRInteractor interactorSeleccionado = null;

    // Devuelve las 3 posiciones de snap en el orden de marcha (0, 1, 2)
    private float[] PosicionesDeSnap() => new[] { xMinima, xMedia, xMaxima };

    void OnEnable()
    {
        if (interactable != null)
        {
            interactable.selectEntered.AddListener(AlAgarrar);
            interactable.selectExited.AddListener(AlSoltar);
        }
    }

    void OnDisable()
    {
        if (interactable != null)
        {
            interactable.selectEntered.RemoveListener(AlAgarrar);
            interactable.selectExited.RemoveListener(AlSoltar);
        }
    }

    void Start()
    {
        if (pomoPalanca != null)
        {
            // Guardamos Y y Z iniciales locales para que nunca se desvie en altura o profundidad
            yFija = pomoPalanca.localPosition.y;
            zFija = pomoPalanca.localPosition.z;

            // Posicionamos la palanca inicialmente en la Fase 0
            AplicarXLocal(xMinima);
        }
    }

    private void AlAgarrar(SelectEnterEventArgs args)
    {
        interactorSeleccionado = args.interactorObject;
    }

    private void AlSoltar(SelectExitEventArgs args)
    {
        interactorSeleccionado = null;

        // Leemos la X real en el instante exacto de soltar (no la marcha detectada durante el
        // arrastre, que podria haber quedado un frame desfasada) y buscamos cual de los 3 puntos
        // de snap definidos (xMinima, xMedia, xMaxima) esta mas cerca: asi la palanca siempre
        // "imanta" hacia el valor mas proximo a donde la soltaste, sin importar si quedaste justo
        // a la mitad entre dos marchas.
        float xActual = (pomoPalanca != null && basePalanca != null)
            ? basePalanca.InverseTransformPoint(pomoPalanca.position).x
            : xMinima;

        marchaActual = MarchaMasCercana(xActual);
        float xObjetivo = PosicionesDeSnap()[marchaActual];

        AplicarXLocal(xObjetivo);

        if (controladorTren != null)
        {
            controladorTren.CambiarPosicionPalanca(marchaActual);
        }

        Debug.Log($"[PALANCA RIGIDA] Snapeado a posicion mas cercana (marcha {marchaActual}): {xObjetivo}");
    }

    void LateUpdate()
    {
        if (pomoPalanca == null || basePalanca == null) return;

        // Mientras se agarra en VR, la posicion EN VIVO del pomo la gestiona EXCLUSIVAMENTE
        // FiltroPalancaVR (el Grab Transformer de XRI). XRI recalcula la pose del objeto agarrado
        // en una fase "Before Render" que corre DESPUES de LateUpdate (para minimizar la latencia
        // de la mano justo antes de dibujar el frame): cualquier posicion que este script fuera a
        // escribir aqui quedaria sobrescrita de todas formas. Aqui solo LEEMOS la X resultante
        // (reconvertida a espacio de 'basePalanca' a partir de la posicion en MUNDO, nunca de
        // 'localPosition': mientras se agarra, XRGrabInteractable desengancha a pomoPalanca de su
        // padre, asi que 'localPosition' no seria relativa a 'basePalanca' en ese momento) para
        // avisar al tren en vivo de la marcha mas cercana segun el recorrido.
        if (interactorSeleccionado != null)
        {
            float xActual = basePalanca.InverseTransformPoint(pomoPalanca.position).x;
            int nuevaMarcha = MarchaMasCercana(xActual);

            if (nuevaMarcha != marchaActual)
            {
                marchaActual = nuevaMarcha;

                if (controladorTren != null)
                {
                    controladorTren.CambiarPosicionPalanca(marchaActual);
                }

                Debug.Log($"[PALANCA RIGIDA] Marcha activada: {marchaActual}");
            }
        }
    }

    private void AplicarXLocal(float nuevoX)
    {
        if (pomoPalanca == null || basePalanca == null) return;

        // IMPORTANTE: mientras XRI tiene agarrado el pomo, XRGrabInteractable lo DESENGANCHA de
        // su padre original (pomoPalanca.parent pasa a null durante el agarre, y se restaura al
        // soltar). Por eso escribir 'localPosition' aqui ya NO seria relativo a 'basePalanca'
        // durante el agarre. Calculamos la posicion deseada en espacio de 'basePalanca' y la
        // convertimos a MUNDO, asi funciona sin importar cual sea el padre real de pomoPalanca
        // en este momento (esto solo se usa fuera del agarre: al iniciar, y al soltar).
        Vector3 posLocalDeseada = new Vector3(nuevoX, yFija, zFija);
        pomoPalanca.position = basePalanca.TransformPoint(posLocalDeseada);
        pomoPalanca.rotation = basePalanca.rotation;
    }

    // Devuelve el indice de marcha (0, 1 o 2) cuya posicion de snap esta MAS CERCA de 'xActual'.
    // Al basarse en distancia real a cada punto (no en umbrales fijos escritos a mano), funciona
    // igual aunque se agreguen mas posiciones de snap en el futuro, y siempre "imanta" hacia el
    // valor mas proximo a donde quedo la palanca.
    private int MarchaMasCercana(float xActual)
    {
        float[] posiciones = PosicionesDeSnap();
        int marchaMasCercana = 0;
        float menorDistancia = Mathf.Abs(xActual - posiciones[0]);

        for (int i = 1; i < posiciones.Length; i++)
        {
            float distancia = Mathf.Abs(xActual - posiciones[i]);
            if (distancia < menorDistancia)
            {
                menorDistancia = distancia;
                marchaMasCercana = i;
            }
        }

        return marchaMasCercana;
    }
}