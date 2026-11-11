using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Transformers;

public class FiltroPalancaVR : XRBaseGrabTransformer
{
    [Header("Referencias")]
    public Transform basePalanca; // El pivote local de la consola

    [Header("Límites del Riel")]
    public float xMin = -0.0654f;
    public float xMax = 0.0693f;

    private float yFija;
    private float zFija;

    // Se ejecuta una vez al asignar el componente
    public override void OnLink(XRGrabInteractable interactable)
    {
        base.OnLink(interactable);
        yFija = interactable.transform.localPosition.y;
        zFija = interactable.transform.localPosition.z;
    }

    // Se ejecuta internamente en el pipeline de XRI antes de mover el objeto
    public override void Process(XRGrabInteractable interactable, XRInteractionUpdateOrder.UpdatePhase updatePhase, ref Pose targetPose, ref Vector3 localScale)
    {
        if (basePalanca == null) return;

        // 1. Traducimos la posición que quiere la mano al espacio local de la consola inclinada
        Vector3 posLocal = basePalanca.InverseTransformPoint(targetPose.position);

        // 2. Encerramos el movimiento rígidamente en el eje X
        posLocal.x = Mathf.Clamp(posLocal.x, xMin, xMax);

        // 3. Anulamos por completo cualquier elevación o desvío forzando Y y Z
        posLocal.y = yFija;
        posLocal.z = zFija;

        // 4. Devolvemos las coordenadas filtradas al mundo. Ahora XRI moverá el pomo sin salirse del riel.
        targetPose.position = basePalanca.TransformPoint(posLocal);
        targetPose.rotation = basePalanca.rotation;
    }
}