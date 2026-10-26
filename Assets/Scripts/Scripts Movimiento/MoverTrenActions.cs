using UnityEngine;
using UnityEngine.InputSystem;

public class MoverTrenActions : MonoBehaviour
{
    public float velocidad = 10f;

    // Se ejecuta automáticamente al accionar 'Adelante' (Teclado W o Mando Arriba)
    public void OnAdelante(InputValue value)
    {
        if (value.isPressed)
        {
            transform.Translate(Vector3.forward * velocidad * Time.deltaTime);
        }
    }

    // Se ejecuta automáticamente al accionar 'Atras' (Tecla S o Mando Abajo)
    public void OnAtras(InputValue value)
    {
        if (value.isPressed)
        {
            transform.Translate(Vector3.back * velocidad * Time.deltaTime);
        }
    }


}