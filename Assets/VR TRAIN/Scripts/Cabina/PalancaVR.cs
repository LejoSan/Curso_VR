using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

// Controla el desplazamiento restringido en X y congela la palanca al soltarla
public class PalancaVR : MonoBehaviour
{
    [Header("Referencias de la Jerarquía")]
    public Transform basePalanca;                      // Objeto 'Base' (Pivote orientado)
    public Transform pomoPalanca;                      // Objeto 'Pomo Palanca'
    public XRGrabInteractable interactable;            // Componente XR Grab Interactable del Pomo
    public ControladorTrenVR controladorTren;        // Script principal del Tren

    [Header("Límites del Recorrido en X Local")]
    public float posicion0 = -0.0654f;                 // Fase 0: Parado
    public float posicion1 = -0.0006f;                 // Fase 1: Velocidad Media
    public float posicion2 = 0.0693f;                  // Fase 2: Velocidad Máxima

    private IXRInteractor interactorSeleccionado = null;
    private int marchaActual = 0;

    // Y y Z locales fijos del pomo dentro de la base
    private float yInicial;
    private float zInicial;

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
            // Memorizamos Y y Z para que el pomo nunca desvíe su trayectoria
            yInicial = pomoPalanca.localPosition.y;
            zInicial = pomoPalanca.localPosition.z;

            // Colocamos el pomo en la posición 0 de inicio
            Vector3 posInicial = pomoPalanca.localPosition;
            posInicial.x = posicion0;
            pomoPalanca.localPosition = posInicial;
        }
    }

    private void AlAgarrar(SelectEnterEventArgs args)
    {
        interactorSeleccionado = args.interactorObject;
    }

    private void AlSoltar(SelectExitEventArgs args)
    {
        // Al soltar la mano, anulamos la referencia del interactor.
        // Al ser IsKinematic = true, el pomo se queda congelado en la posición X actual.
        interactorSeleccionado = null;
    }

    void LateUpdate()
    {
        if (pomoPalanca == null || basePalanca == null) return;

        // Mientras la mano mantenga sujetado el pomo en VR:
        if (interactorSeleccionado != null)
        {
            // 1. Obtenemos la posición de la mano en coordenadas locales de la Base
            Vector3 posManoMundo = interactorSeleccionado.transform.position;
            Vector3 posManoLocal = basePalanca.InverseTransformPoint(posManoMundo);

            // 2. Restringimos únicamente el eje X entre posicion0 (-0.0654) y posicion2 (0.0693)
            float xRestringido = Mathf.Clamp(posManoLocal.x, posicion0, posicion2);

            // 3. Forzamos la posición local del pomo manteniendo Y y Z bloqueados
            pomoPalanca.localPosition = new Vector3(xRestringido, yInicial, zInicial);

            // 4. Notificamos la marcha al controlador del tren
            EvaluarMarcha(xRestringido);
        }
    }

    private void EvaluarMarcha(float xActual)
    {
        float umbral0a1 = (posicion0 + posicion1) / 2.0f; // -0.033
        float umbral1a2 = (posicion1 + posicion2) / 2.0f; //  0.0343

        int nuevaMarcha = 0;

        if (xActual < umbral0a1) nuevaMarcha = 0;
        else if (xActual >= umbral0a1 && xActual < umbral1a2) nuevaMarcha = 1;
        else if (xActual >= umbral1a2) nuevaMarcha = 2;

        if (nuevaMarcha != marchaActual)
        {
            marchaActual = nuevaMarcha;

            if (controladorTren != null)
            {
                controladorTren.CambiarPosicionPalanca(marchaActual);
            }

            Debug.Log($"[TREN VR]: Marcha cambiada a {marchaActual}");
        }
    }
}