using UnityEngine;

public class BucleFor : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //for (int i = 1; i <= 5; i = i + 1) 
        //{
        //    Debug.Log("Revisando vagon numero :  " + i);
        //}

        //for (int velocidad = 0; velocidad <= 100; velocidad += 20)
        //{
        //    Debug.Log("Velocidad del tren: " + velocidad + "Km/h");
        //}

        //for (int freno = 5;  freno >= 0; freno --)
        //{
        //    Debug.Log("Liberando presion de freno: = " + freno);
        //}

        for (int i = 1; i < 4; i++)
        {
            Debug.Log("puerta vagon  " + i + " comprobada ");
        }

        for (int i = 1; i <= 2; i++)
        {
            Debug.Log("Faro del tren   " + i + " encendido ");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
