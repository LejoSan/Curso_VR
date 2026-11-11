using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

// Este script controla el movimiento rígido en X local, imanta (snapea) al soltar y previene que la palanca se eleve en movimiento
public class PalancaRigidaVR : MonoBehaviour
{
    [Header("Referencias de Jerarquía")]
    public Transform basePalanca;                   // Objeto 'Base' (Pivote local orientable)
    public Transform pomoPalanca;                   // Objeto 'Pomo_Palanca'
    public XRGrabInteractable interactable;         // XR Grab Interactable del Pomo
    public ControladorTrenVR controladorTren;     // Script del Tren

    [Header("Posiciones Exactas de Snap en X Local")]
    public float xMinima = -0.0654f;                // Fase 0 (Abajo / Detenido)
    public float xMedia = -0.0006f;                // Fase 1 (Medio / Vel. Media)
    public float xMaxima = 0.0693f;                // Fase 2 (Arriba / Vel. Máxima)

    private float yFija;
    private float zFija;
    private int marchaActual = 0;
    private IXRInteractor interactorSeleccionado = null;

    // Referencia de agarre: medimos el movimiento de la mano como un DELTA en espacio mundial
    // desde el instante en que se agarra, en vez de reconvertir su posición absoluta contra
    // 'basePalanca' cada frame. 'basePalanca' viaja dentro del tren (solo se traslada, no rota
    // mientras avanza), así que reconstruir la posición local absoluta cada frame mezclaba la
    // posición de la mano (recién muestreada) con un marco de referencia que ya se había
    // desplazado ese mismo frame, acumulando un desvío proporcional a la velocidad del tren.
    // Usando un delta en mundo proyectado sobre la dirección del riel (un vector de dirección,
    // no una posición) ese desvío se cancela por completo.
    private Vector3 manoMundoAlAgarrar;
    private float xAlAgarrar;
    private Vector3 direccionRielMundo;

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
            // Guardamos Y y Z iniciales locales para que nunca se desvíe en altura o profundidad
            yFija = pomoPalanca.localPosition.y;
            zFija = pomoPalanca.localPosition.z;

            // Posicionamos la palanca inicialmente en la Fase 0
            AplicarXLocal(xMinima);
        }
    }

    private void AlAgarrar(SelectEnterEventArgs args)
    {
        interactorSeleccionado = args.interactorObject;

        // Punto de partida del delta: posición de la mano en mundo, X actual del pomo,
        // y la dirección del riel en mundo (se recalcula por si 'basePalanca' tiene una
        // orientación fija distinta cada vez, aunque durante el agarre no debería rotar).
        manoMundoAlAgarrar = interactorSeleccionado.transform.position;

        // Leemos la X de partida en espacio de 'basePalanca' a partir de la posición en MUNDO del
        // pomo (no de su localPosition): XRI puede desenganchar pomoPalanca de su padre justo al
        // iniciar el agarre, y en ese caso 'localPosition' ya no sería relativa a 'basePalanca'.
        xAlAgarrar = (pomoPalanca != null && basePalanca != null)
            ? basePalanca.InverseTransformPoint(pomoPalanca.position).x
            : 0f;
        direccionRielMundo = basePalanca != null ? basePalanca.TransformDirection(Vector3.right) : Vector3.right;
    }

    private void AlSoltar(SelectExitEventArgs args)
    {
        interactorSeleccionado = null;

        // SNAP AL SOLTAR: Encaja automáticamente la palanca en el punto exacto de la marcha actual
        float xObjetivo = xMinima;
        if (marchaActual == 1) xObjetivo = xMedia;
        else if (marchaActual == 2) xObjetivo = xMaxima;

        AplicarXLocal(xObjetivo);
        Debug.Log($"[PALANCA RÍGIDA] Snapeado a posición exacta de marcha {marchaActual}: {xObjetivo}");
    }

    void LateUpdate()
    {
        if (pomoPalanca == null || basePalanca == null) return;

        // Mientras se agarra en VR, la posición EN VIVO del pomo la gestiona EXCLUSIVAMENTE
        // FiltroPalancaVR (el Grab Transformer de XRI). Esto no es opcional: XRI recalcula la
        // pose del objeto agarrado en una fase "Before Render" que corre DESPUÉS de LateUpdate
        // (para minimizar la latencia de la mano justo antes de dibujar el frame), así que
        // cualquier posición que este script fuera a escribir aquí quedaría sobrescrita de todas
        // formas. El Grab Transformer sí participa en esa fase, así que es el único lugar correcto
        // para aplicar el clamp del riel. Aquí solo LEEMOS la X resultante (reconvertida a espacio
        // de 'basePalanca' a partir de la posición en MUNDO, nunca de 'localPosition': mientras se
        // agarra, XRGrabInteractable desengancha a pomoPalanca de su padre, así que 'localPosition'
        // no sería relativa a 'basePalanca' en ese momento) para evaluar la marcha activa.
        if (interactorSeleccionado != null)
        {
            float xActual = basePalanca.InverseTransformPoint(pomoPalanca.position).x;
            EvaluarMarcha(xActual);
        }
    }

    private void AplicarXLocal(float nuevoX)
    {
        if (pomoPalanca == null || basePalanca == null) return;

        // IMPORTANTE: mientras XRI tiene agarrado el pomo, XRGrabInteractable lo DESENGANCHA
        // de su padre original (pomoPalanca.parent pasa a null durante el agarre, y se restaura
        // al soltar). Por eso escribir 'localPosition' aquí ya NO es relativo a 'basePalanca'
        // durante el agarre: es básicamente una posición de mundo cerca del origen, lo que hacía
        // que la palanca "saltara" a una posición incorrecta en vez de quedarse en el riel.
        // Calculamos la posición deseada en espacio de 'basePalanca' y la convertimos a MUNDO,
        // así funciona sin importar cuál sea el padre real de pomoPalanca en este momento.
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

            Debug.Log($"[PALANCA RÍGIDA] Marcha activada: {marchaActual}");
        }
    }
}