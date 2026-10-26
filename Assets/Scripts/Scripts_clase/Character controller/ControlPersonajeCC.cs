using UnityEngine;
using UnityEngine.InputSystem;


public class ControlPersonajeCC : MonoBehaviour
{
    public float velocidad = 6f;

    private CharacterController controller;
    private Vector2 inputDireccion;

    void Start()
    {
        // Guarda la referencia del CharacterController adjunto
        controller = GetComponent<CharacterController>();
    }

    // Se ejecuta con el Nuevo Input System al mover WASD o el Stick
    public void OnMove(InputValue valor)
    {
        inputDireccion = valor.Get<Vector2>();
    }

    void Update()
    {
        // 1. Convertimos la entrada 2D a un Vector3 en el mundo 3D
        // (transform.right = Eje X local | transform.forward = Eje Z local)
        Vector3 direccion3D = (transform.right * inputDireccion.x) + (transform.forward * inputDireccion.y);

        // 2. Aplicamos el movimiento mediante CharacterController.Move()
        controller.Move(direccion3D * velocidad * Time.deltaTime);
    }
}