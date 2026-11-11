using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class teleportationactivate : MonoBehaviour
{
    public XRRayInteractor Rayoteletrasportado;
    public InputActionProperty Botonteleport;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Rayoteletrasportado.gameObject.SetActive(false);

        Botonteleport.action.performed += ActivateRayo;
    }

    private void ActivateRayo(InputAction.CallbackContext obj)
    {
        Rayoteletrasportado.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Botonteleport.action.WasReleasedThisFrame()) {

         Rayoteletrasportado.gameObject.SetActive(false);

        }

    }
}
