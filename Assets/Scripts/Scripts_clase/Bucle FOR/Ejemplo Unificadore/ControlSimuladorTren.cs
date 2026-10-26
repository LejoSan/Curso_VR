using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlSimuladorTren : MonoBehaviour
{

    //Variables 

    public int Ntotalvagones = 5;
    public bool puertacerrada = false;
    public float VelocidadActual = 0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log(" BIENVENIDOS AL EL TREN ARF MADRID ");

        // agregamos un for para verificar los vagones al iniciar el programa 
        for (int i = 1; i <= Ntotalvagones; i++)
        {
            Debug.Log("Inspeccionando vagon " + i + ".... OK");
        }

        Debug.Log("Vagones inspeccionados procede cerrar las puertas ( DESDE EL INSPECTOR ) ");
    }

    // Update is called once per frame
    void Update()
    {
        // condicion 

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ArrancarTren();
        }

        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            FrenarTren();
        }
    }

    private void FrenarTren()
    {
        Debug.Log("Frenando....");

        while(VelocidadActual > 0f)
        {
            VelocidadActual -= 20f;

            Debug.Log("Velocidad: " + VelocidadActual + "Km/h");
        }
    }

    private void ArrancarTren()
    {
        if (puertacerrada)
        {
            Debug.Log("puertas seguras. Acelerando.....");

            //bucle while 
            while (VelocidadActual < 80f)
            {
                VelocidadActual += 20f;
                Debug.Log("Velocidad: " +VelocidadActual + "Km/h");

            }

        }
        else
        {
            Debug.Log("OJO! LAS PUERTAS ESTAN ABIERTAS - PULSA EN LA CASILLA DEL INSPECTOR ANTES DE PONERTE EN MARCHA ");
        }
    }
}
