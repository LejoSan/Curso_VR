using UnityEngine;

public class velocidatren : MonoBehaviour
{
    //float velocidad = 50f;

    void Start()
    {
        // Bucle Padre: Recorre 3 trenes en la estación
        for (int i = 1; i <= 3; i++)
        {
            Debug.Log("Comienza la inspección del Tren Nº " + i);

            // Bucle Hijo: Recorre 4 vagones por cada tren
            for (int j = 1; j <= 4; j++)
            {
                Debug.Log("  -> Vagón " + j + " del Tren " + i + " verificado");
            }
        }
    }
}
