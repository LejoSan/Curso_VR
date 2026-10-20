using UnityEngine;
using UnityEngine.Video;

public class miprimerscript : MonoBehaviour
{
        
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        float velocidaddeltren = CalcularlavelocidadTren(100f,20f);
       
        Debug.Log("la velocidad del tren es : " + velocidaddeltren);

        if (velocidaddeltren > 0)
        {
            Debug.Log("EL tren se esta moviendo");
        }
        if (velocidaddeltren >= 100)
        {
            Debug.Log("EL tren se mueve muy rapido ");
        }
        if (velocidaddeltren <= 0)
        {
            Debug.Log("EL tren detenido ");
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    float CalcularlavelocidadTren(float distancia, float tiempo) {

        float velocidad = distancia / tiempo;
        //Debug.Log("La velocidad es: " + velocidad);
        return velocidad;
    }
}
