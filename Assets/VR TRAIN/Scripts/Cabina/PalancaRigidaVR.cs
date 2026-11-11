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

        // Únicamente actualizamos posición mientras la palanca esté agarrada en VR
        if (interactorSeleccionado != null)
        {
            // 1. Convertimos la posición de la mano en el mundo a espacio local del contenedor Base
            Vector3 posManoMundo = interactorSeleccionado.transform.position;
            Vector3 posManoLocal = basePalanca.InverseTransformPoint(posManoMundo);

            // 2. Clampeamos estrictamente la posición de la mano dentro del rango X local (-0.0654 a 0.0693)
            float xClamped = Mathf.Clamp(posManoLocal.x, xMinima, xMaxima);

            // 3. Forzamos la posición manteniendo Y y Z congelados (evita que el tubo se eleve)
            AplicarXLocal(xClamped);

            // 4. Evaluamos la marcha activa según el recorrido
            EvaluarMarcha(xClamped);
        }
    }

    private void AplicarXLocal(float nuevoX)
    {
        if (pomoPalanca != null)
        {
            pomoPalanca.localPosition = new Vector3(nuevoX, yFija, zFija);
        }
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