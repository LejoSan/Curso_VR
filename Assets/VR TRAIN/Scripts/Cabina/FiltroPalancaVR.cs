using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Transformers;

public class FiltroPalancaVR : XRBaseGrabTransformer
{
    [Header("Referencias")]
    public Transform basePalanca; // El pivote local de la consola

    [Header("Limites del Riel")]
    public float xMin = -0.0654f;
    public float xMax = 0.0693f;

    private float yFija;
    private float zFija;

    // Referencia de agarre tomada UNA VEZ al iniciar el agarre (ver OnGrab): posicion de la mano
    // en MUNDO, la posicion de 'basePalanca' en MUNDO (en ese mismo instante), y la X que tenia
    // el pomo en ese instante (relativa a 'basePalanca'). 'basePalanca' viaja dentro del tren (se
    // traslada, no rota, mientras avanza). El movimiento real de la mano, descontando el que viene
    // solo de que el tren arrastre consigo tanto la mano como la base, es:
    //
    //   realMovimiento = (manoAhora - baseAhora) - (manoAlAgarrar - baseAlAgarrar)
    //
    // Restar la posicion de la base en CADA uno de los dos instantes (no solo usar una direccion
    // fija) es necesario porque el riel de esta palanca NO es perpendicular a la direccion de
    // avance del tren (en este panel esta casi alineado con ella), asi que proyectar el delta de
    // la mano sobre una sola direccion fija dejaba pasar la mayor parte del desplazamiento del
    // tren como si fuera un movimiento real de la mano, acumulando un desvio cada vez mayor.
    private Vector3 manoMundoAlAgarrar;
    private Vector3 baseMundoAlAgarrar;
    private float xAlAgarrar;
    private Vector3 direccionRielMundo;

    // Se ejecuta una vez al asignar el componente
    public override void OnLink(XRGrabInteractable interactable)
    {
        base.OnLink(interactable);
        yFija = interactable.transform.localPosition.y;
        zFija = interactable.transform.localPosition.z;
    }

    // Se ejecuta UNA VEZ cada vez que comienza un agarre (nuevo interactor seleccionando)
    public override void OnGrab(XRGrabInteractable grabInteractable)
    {
        base.OnGrab(grabInteractable);
        if (basePalanca == null) return;

        IXRSelectInteractor interactor = grabInteractable.interactorsSelecting.Count > 0
            ? grabInteractable.interactorsSelecting[grabInteractable.interactorsSelecting.Count - 1]
            : null;

        manoMundoAlAgarrar = interactor != null
            ? interactor.GetAttachTransform(grabInteractable).position
            : grabInteractable.transform.position;

        baseMundoAlAgarrar = basePalanca.position;
        xAlAgarrar = basePalanca.InverseTransformPoint(grabInteractable.transform.position).x;
        direccionRielMundo = basePalanca.TransformDirection(Vector3.right);
    }

    // Se ejecuta internamente en el pipeline de XRI antes de mover el objeto (incluye la fase
    // "Before Render" de baja latencia, que corre DESPUES de LateUpdate: por eso el clamp tiene
    // que vivir aqui y no en un script con LateUpdate normal).
    public override void Process(XRGrabInteractable interactable, XRInteractionUpdateOrder.UpdatePhase updatePhase, ref Pose targetPose, ref Vector3 localScale)
    {
        if (basePalanca == null) return;

        IXRSelectInteractor interactor = interactable.interactorsSelecting.Count > 0
            ? interactable.interactorsSelecting[interactable.interactorsSelecting.Count - 1]
            : null;
        if (interactor == null) return;

        // 1. Movimiento REAL de la mano relativo al tren: restamos a cada posicion de la mano
        //    (ahora y al agarrar) la posicion de 'basePalanca' en ESE MISMO instante. Asi, el
        //    desplazamiento que viene de que el tren arrastre consigo tanto la mano como la base
        //    se cancela por completo sin importar si el riel es paralelo o perpendicular a la
        //    direccion de avance.
        Vector3 manoMundoActual = interactor.GetAttachTransform(interactable).position;
        Vector3 movimientoReal = (manoMundoActual - basePalanca.position) - (manoMundoAlAgarrar - baseMundoAlAgarrar);

        // 2. Proyectamos ese movimiento real sobre la direccion del riel.
        float deltaRiel = Vector3.Dot(movimientoReal, direccionRielMundo);

        // 3. Aplicamos ese delta sobre la X que tenia el pomo al agarrarla, y clamp al rango.
        float xClamped = Mathf.Clamp(xAlAgarrar + deltaRiel, xMin, xMax);

        // 4. Construimos la pose final en espacio de 'basePalanca' (Y/Z fijos, sin desviacion)
        //    y la convertimos a mundo.
        Vector3 posLocal = new Vector3(xClamped, yFija, zFija);
        targetPose.position = basePalanca.TransformPoint(posLocal);
        targetPose.rotation = basePalanca.rotation;
    }
}