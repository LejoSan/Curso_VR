using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class TeleportationActivator : MonoBehaviour
{
    public XRRayInteractor teleporRay;
    public InputActionProperty teleportButton;
    public XRRayInteractor rayInteractor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        teleporRay.gameObject.SetActive(false);
        teleportButton.action.performed += ActivarRayo;
        rayInteractor.uiHoverEntered.AddListener(DisableRay);
    }
    
    private void ActivarRayo(InputAction.CallbackContext obj)
    {
        if (rayInteractor && rayInteractor.IsOverUIGameObject())
        {
            return;
        }

        teleporRay.gameObject.SetActive(true);
    }

    private void DisableRay(UIHoverEventArgs args)
    {
        teleporRay.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (teleportButton.action.WasReleasedThisFrame())
        {
            teleporRay.gameObject.SetActive(false);
        }
    }
}
