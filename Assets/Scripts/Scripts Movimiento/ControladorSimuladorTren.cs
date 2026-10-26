using UnityEngine;
using UnityEngine.InputSystem; // Importante para usar el nuevo Input System

public class ControladorSimuladorTren : MonoBehaviour
{
    // 1. VARIABLES (Public = editables en el Inspector)
    [Header("Configuración del Tren")]
    public int totalVagones = 4;
    public bool puertasCerradas = false; // Casilla a marcar en el Inspector
    public float velocidadActual = 0f;

    // Start: Se ejecuta 1 vez al pulsar Play
    void Start()
    {
        Debug.Log(" SIMULADOR DE TREN ");

        // BUCLE FOR: Inspecciona los vagones al iniciar
        for (int i = 1; i <= totalVagones; i++)
        {
            Debug.Log("Inspeccionando vagón " + i + "... OK");
        }

        Debug.Log("Marca la casilla 'Puertas Cerradas' en el Inspector y pulsa [ESPACIO] para arrancar.");
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        // Tecla ESPACIO -> Intenta arrancar (1 solo frame)
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ArrancarTren();
        }

        // Tecla F -> Frena el tren (1 solo frame)
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            FrenarTren();
        }
    }

    void ArrancarTren()
    {
        // CONDICIONAL IF: Verifica seguridad
        if (puertasCerradas)
        {
            Debug.Log("Puertas seguras. Acelerando...");

            // BUCLE WHILE: Aumenta la velocidad hasta 80 km/h
            while (velocidadActual < 80f)
            {
                velocidadActual += 20f;
                Debug.Log("Velocidad: " + velocidadActual + " km/h");
            }
        }
        else
        {
            Debug.Log(" OJO! Marca 'Puertas Cerradas' en el Inspector antes de arrancar.");
        }
    }

    void FrenarTren()
    {
        Debug.Log("Aplicando frenos...");

        // BUCLE WHILE: Reduce la velocidad hasta 0 km/h
        while (velocidadActual > 0f)
        {
            velocidadActual -= 20f;
            Debug.Log("Velocidad: " + velocidadActual + " km/h");
        }
    }
}