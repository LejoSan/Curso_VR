using UnityEngine;
using UnityEngine.Video;

public class miprimerscript : MonoBehaviour
{

    public float distancia = 500f;
    public float tiempo = 20f;
    float velocidad;
        
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        CalcularlavelocidadTren();
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CalcularlavelocidadTren() {

        velocidad = distancia / tiempo;
        Debug.Log("La velocidad es: " + velocidad);
    }
}
