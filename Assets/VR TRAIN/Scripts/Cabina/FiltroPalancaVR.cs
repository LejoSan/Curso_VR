using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Transformers;

public class FiltroPalancaVR : XRBaseGrabTransformer
{
    public Transform basePalanca;
    public float xMin = -0.0654f;
    public float xMax = 0.0693f;

    private float yFija;
    private float zFija;

    private Vector3 manoMundoAlAgarrar;
    private Vector3 baseMundoAlAgarrar;
    private float xAlAgarrar;
    private Vector3 direccionRielMundo;

    public override void OnLink(XRGrabInteractable interactable)
    {
        base.OnLink(interactable);

        yFija = interactable.transform.localPosition.y;
        zFija = interactable.transform.localPosition.z;
    }

    public override void OnGrab(XRGrabInteractable grabInteractable)
    {
        base.OnGrab(grabInteractable);

        if (basePalanca == null)
        {
            return;
        }

        IXRSelectInteractor interactor = null;
        int cantidadManos = grabInteractable.interactorsSelecting.Count;

        if (cantidadManos > 0)
        {
            interactor = grabInteractable.interactorsSelecting[cantidadManos - 1];
        }

        if (interactor != null)
        {
            manoMundoAlAgarrar = interactor.GetAttachTransform(grabInteractable).position;
        }
        else
        {
            manoMundoAlAgarrar = grabInteractable.transform.position;
        }

        baseMundoAlAgarrar = basePalanca.position;
        xAlAgarrar = basePalanca.InverseTransformPoint(grabInteractable.transform.position).x;
        direccionRielMundo = basePalanca.TransformDirection(Vector3.right);
    }

    public override void Process(XRGrabInteractable interactable, XRInteractionUpdateOrder.UpdatePhase updatePhase, ref Pose targetPose, ref Vector3 localScale)
    {
        if (basePalanca == null)
        {
            return;
        }

        IXRSelectInteractor interactor = null;
        int cantidadManos = interactable.interactorsSelecting.Count;

        if (cantidadManos > 0)
        {
            interactor = interactable.interactorsSelecting[cantidadManos - 1];
        }

        if (interactor == null)
        {
            return;
        }

        Vector3 manoMundoActual = interactor.GetAttachTransform(interactable).position;
        Vector3 distanciaActualManoABase = manoMundoActual - basePalanca.position;
        Vector3 distanciaInicialManoABase = manoMundoAlAgarrar - baseMundoAlAgarrar;

        Vector3 movimientoReal = distanciaActualManoABase - distanciaInicialManoABase;

        float deltaRiel = Vector3.Dot(movimientoReal, direccionRielMundo);

        float nuevaXSinLimites = xAlAgarrar + deltaRiel;
        float xClamped = Mathf.Clamp(nuevaXSinLimites, xMin, xMax);

        Vector3 posLocal = new Vector3(xClamped, yFija, zFija);
        targetPose.position = basePalanca.TransformPoint(posLocal);
        targetPose.rotation = basePalanca.rotation;
    }
}