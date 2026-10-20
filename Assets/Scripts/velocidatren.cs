using UnityEngine;

public class velocidatren : MonoBehaviour
{
    float velocidad = 50f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        while (velocidad < 100f)
        {
            velocidad += 20f;

            Debug.Log("Velocidad del tren: " + velocidad + " km/h");
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
