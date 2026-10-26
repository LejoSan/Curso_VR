using UnityEngine;

public class interaccion : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //funcionteclado();
        //funcionRaton();
        //Movimientopersonaje();

        //--  disparo personaje --

        // click izquierdo (0) y NO esta haciendo click (!1)
        if (Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1))
        {
            Debug.Log("Disparo sin apuntar");
        }

        //Mantiene Clic derecho(1) para apuntar
        if (Input.GetMouseButton(1)){

            Debug.Log("APUNTADO");

            //Desparar estadno en modo apuntar 

            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Disparo apuntado");

            }
        }
                

    }

    void funcionteclado() {

        //  ------ TECLADO --------

        // Al presionar la tecla (1 frame)
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Se pulsa tecla A");
        }

        // Al soltar la tecla (1 frame)
        if (Input.GetKeyUp(KeyCode.A))
        {
            Debug.Log("Se levanta la tecla A");
        }

        // Al soltar la tecla (1 frame)
        if (Input.GetKey(KeyCode.M))
        {
            Debug.Log("Se mantiene pulsada la letra M");
        }



    }

    void funcionRaton()
    {

        //  ------ RATON --------

        if (Input.GetMouseButtonDown(0))
        {

            Debug.Log("Se pulsa click izquierdo del raton ");
        }

        if (Input.GetMouseButtonUp(0))
        {

            Debug.Log("Se levanta click izquierdo del raton ");
        }

        if (Input.GetMouseButton(0))
        {

            Debug.Log("Se mantiene el  click izquierdo del raton ");
        }
    }


    void Movimientopersonaje()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {

            //----caminando---
            Debug.Log("Me muevo hacia delante");

            if (Input.GetKey(KeyCode.LeftShift))
            {

                // ---correr----
                Debug.Log("Estoy corriendo");
            }

        }
    }
}
