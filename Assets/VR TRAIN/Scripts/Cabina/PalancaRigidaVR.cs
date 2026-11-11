using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

// Este script NO mueve el pomo (eso lo hace FiltroPalancaVR, el Grab Transformer de XRI, que
// corre en todas las fases del pipeline de interaccion incluida "Before Render"). Aqui solo
// LEEMOS la X resultante para detectar la marcha activa, y aplicamos el snap exacto al soltar.
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

        // SNAP AL SOLTAR: Encaja automaticamente la palanca en el punto exacto de la marcha actual
        float xObjetivo = xMinima;
        if (marchaActual == 1) xObjetivo = xMedia;
        else if (marchaActual == 2) xObjetivo = xMaxima;

        AplicarXLocal(xObjetivo);
        Debug.Log($"[PALANCA RIGIDA] Snapeado a posicion exacta de marcha {marchaActual}: {xObjetivo}");
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
        // evaluar la marcha activa.
        if (interactorSeleccionado != null)
        {
            float xActual = basePalanca.InverseTransformPoint(pomoPalanca.position).x;
            EvaluarMarcha(xActual);
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

    private void EvaluarMarcha(float xActual)
    {
        float umbral0a1 = (xMinima + xMedia) / 2.0f; // -0.033
        float umbral1a2 = (xMedia + xMaxima) / 2.0f; //  0.0343

        int nuevaMarcha = 0;

        if (xActual < umbral0a1)
        {
            nuevaMarcha = 0;
        }
        else if (xActual >= umbral0a1 && xActual < umbral1a2)
        {
            nuevaMarcha = 1;
        }
        else if (xActual >= umbral1a2)
        {
            nuevaMarcha = 2;
        }

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