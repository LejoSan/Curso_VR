using UnityEngine;

public class EjemplosYErroresArrays : MonoBehaviour
{
    int[] miArraySinLongitud;

    void Start()
    {
        // --- EJEMPLO 1 ---
        int[] miArray1 = { 3, 5, 2, 1 };
        miArray1[3] = miArray1[0];
        Debug.Log("Ejemplo 1: " + miArray1[0] + ", " + miArray1[1] + ", " + miArray1[2] + ", " + miArray1[3]);

        // --- EJEMPLO 2 ---
        int[] miArray2 = { 3, 5, 2, 1 };
        int[] miArray2_B = { 3, 4, 9, 8, 6, 3 };
        miArray2[0] = miArray2_B[1] - miArray2[2];
        Debug.Log("Ejemplo 2: " + miArray2[0] + ", " + miArray2[1] + ", " + miArray2[2] + ", " + miArray2[3]);

        // --- EJEMPLO 3 ---
        int[] miArray3 = { 3, 5, 2, 1 };
        miArray3[1] = 2;
        miArray3[0] = miArray3[1];
        Debug.Log("Ejemplo 3: " + miArray3[0] + ", " + miArray3[1] + ", " + miArray3[2] + ", " + miArray3[3]);

        // --- EJEMPLO 4 ---
        int[] miArray4 = { 3, 5, 2, 1 };
        int[] miArray4_B = { 3, 4, 9, 8, 6, 3 };
        miArray4[1] = miArray4[4];
        Debug.Log("Ejemplo 4: " + miArray4[0] + ", " + miArray4[1] + ", " + miArray4[2] + ", " + miArray4[3]);

        // --- ERROR 1: Sin declarar longitud (NullReferenceException) ---
        miArraySinLongitud[4] = 5;
        Debug.Log("Error 1: " + miArraySinLongitud[4]);

        // --- ERROR 2: Índice que no existe (IndexOutOfRangeException) ---
        int[] miArrayErr1 = { 3, 5, 2, 1 };
        int[] miArrayErr2 = { 3, 4, 9, 8, 6, 3 };
        miArrayErr1[1] = miArrayErr1[4];
        Debug.Log("Error 2: " + miArrayErr1[1]);
    }
}