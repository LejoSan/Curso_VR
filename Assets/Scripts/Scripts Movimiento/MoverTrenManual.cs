using UnityEngine;
using UnityEngine.InputSystem; // Importante para usar el nuevo Input System

public class MoverTrenManual : MonoBehaviour
{
    public float velocidad = 10f; // Velocidad de movimiento del tren

    void Update()
    {
        // 1. AVANZAR: Presionar 'W' en Teclado O Inclinación hacia arriba en Mando
        if (Keyboard.current.wKey.isPressed )
        {
            transform.Translate(Vector3.forward * velocidad * Time.deltaTime);
        }

        // 2. RETROCEDER: Presionar 'S' en Teclado O Inclinación hacia abajo en Mando
        if (Keyboard.current.sKey.isPressed)
        {
            transform.Translate(Vector3.back * velocidad * Time.deltaTime);
        }
    }
}