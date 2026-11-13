using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PalancaRigidaVR : MonoBehaviour
{
    public Transform basePalanca;
    public Transform pomoPalanca;
    public XRGrabInteractable interactable;
    public ControladorTrenVR controladorTren;

    public float xMinima = -0.0654f;
    public float xMedia = -0.0006f;
    public float xMaxima = 0.0693f;

    private float yFija;
    private float zFija;
    private int marchaActual = 0;
    private IXRInteractor interactorSeleccionado = null;

    private float[] ObtenerPosicionesDeSnap()
    {
        float[] posiciones = new float[3];
        posiciones[0] = xMinima;
        posiciones[1] = xMedia;
        posiciones[2] = xMaxima;
        return posiciones;
    }

    private void OnEnable()
    {
        if (interactable != null)
        {
            interactable.selectEntered.AddListener(AlAgarrar);
            interactable.selectExited.AddListener(AlSoltar);
        }
    }

    private void OnDisable()
    {
        if (interactable != null)
        {
            interactable.selectEntered.RemoveListener(AlAgarrar);
            interactable.selectExited.RemoveListener(AlSoltar);
        }
    }

    private void Start()
    {
        if (pomoPalanca != null)
        {
            yFija = pomoPalanca.localPosition.y;
            zFija = pomoPalanca.localPosition.z;

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

        float xActual = xMinima;

        if (pomoPalanca != null && basePalanca != null)
        {
            Vector3 posicionLocalEnBase = basePalanca.InverseTransformPoint(pomoPalanca.position);
            xActual = posicionLocalEnBase.x;
        }

        marchaActual = MarchaMasCercana(xActual);

        float[] listaDePosiciones = ObtenerPosicionesDeSnap();
        float xObjetivo = listaDePosiciones[marchaActual];

        AplicarXLocal(xObjetivo);

        if (controladorTren != null)
        {
            controladorTren.CambiarPosicionPalanca(marchaActual);
        }

        Debug.Log($"[PALANCA RIGIDA] Snapeado a posicion mas cercana (marcha {marchaActual}): {xObjetivo}");
    }

    private void LateUpdate()
    {
        if (pomoPalanca == null || basePalanca == null)
        {
            return;
        }

        if (interactorSeleccionado != null)
        {
            Vector3 posicionLocalEnBase = basePalanca.InverseTransformPoint(pomoPalanca.position);
            float xActual = posicionLocalEnBase.x;

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
        if (pomoPalanca == null || basePalanca == null)
        {
            return;
        }

        Vector3 posLocalDeseada = new Vector3(nuevoX, yFija, zFija);
        pomoPalanca.position = basePalanca.TransformPoint(posLocalDeseada);
        pomoPalanca.rotation = basePalanca.rotation;
    }

    private int MarchaMasCercana(float xActual)
    {
        float[] posiciones = ObtenerPosicionesDeSnap();
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