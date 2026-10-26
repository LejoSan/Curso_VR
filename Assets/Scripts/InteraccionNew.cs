using UnityEngine;
using UnityEngine.InputSystem;

public class InteraccionNew : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Clic izquierdo Y NO está haciendo Clic derecho
        if (Mouse.current.leftButton.wasPressedThisFrame && !Mouse.current.rightButton.isPressed)
        {
            Debug.Log("Disparo SIN apuntar");
        }

        // Mantiene Clic derecho para apuntar
        if (Mouse.current.rightButton.isPressed)
        {
            Debug.Log("APUNTO");

            // Dispara estando en modo apuntar
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Debug.Log("Disparo APUNTANDO");
            }
        }




















        // Operador O (||): Permite moverte con 'W' o 'Flecha Arriba'
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
        {
            Debug.Log("Me muevo hacia delante");

            // Condición anidada: Si además presiona Shift Izquierdo, corre
            if (Keyboard.current.leftShiftKey.isPressed)
            {
                Debug.Log("Ando hacia delante y voy corriendo");
            }
        }




















        // Al hacer clic izquierdo (1 solo frame)
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("Se pulsa click izquierdo");
        }

        // Al soltar el clic izquierdo (1 solo frame)
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            Debug.Log("Se levanta click izquierdo");
        }

        // Mientras se mantiene el clic presionado
        if (Mouse.current.leftButton.isPressed)
        {
            Debug.Log("Se mantiene click izquierdo");
        }























        //teclado 
        // Al presionar la tecla (1 solo frame)







        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            Debug.Log("Se pulsa tecla A");
        }

        // Al soltar la tecla (1 solo frame)
        if (Keyboard.current.aKey.wasReleasedThisFrame)
        {
            Debug.Log("Se levanta tecla A");
        }

        // Mientras se mantiene presionada (cada frame)
        if (Keyboard.current.aKey.isPressed)
        {
            Debug.Log("Se mantiene pulsada tecla A");
        }
    }
}
