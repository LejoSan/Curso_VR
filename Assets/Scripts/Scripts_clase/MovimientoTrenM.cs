using UnityEngine;
using UnityEngine.InputSystem; 

public class MovimientoTrenM : MonoBehaviour
{
    public float velocidad = 10f;

    private bool moviendoAdelante;
    private bool moviendoAtras;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (moviendoAdelante)
        {
            Debug.Log("Avanzando...");
            transform.Translate(Vector3.forward * velocidad * Time.deltaTime);
        }

        if (moviendoAtras)
        {
            Debug.Log("Retrocediendo...");
            transform.Translate(Vector3.back * velocidad * Time.deltaTime);
        }

        // Acanzar : presionamos W en el teclado 
        //if (Keyboard.current.wKey.isPressed || Gamepad.current.leftStick.y.ReadValue() > 0.1f )
        //{
        //    Debug.Log("Esta pulsando la tecla w");
        //    transform.Translate(Vector3.forward * velocidad * Time.deltaTime);
        //}

        //// Acanzar : presionamos S en el teclado 
        //if (Keyboard.current.sKey.isPressed || Gamepad.current.leftStick.y.ReadValue() < - 0.1f)
        //{
        //    Debug.Log("Esta pulsando la tecla S");
        //    transform.Translate(Vector3.back * velocidad * Time.deltaTime);
        //}
    }


    public void OnAdelante(InputValue valor)
    {
        moviendoAdelante = valor.isPressed;
    }

    public void OnAtras(InputValue valor)
    {
        moviendoAtras = valor.isPressed;
    }
}
