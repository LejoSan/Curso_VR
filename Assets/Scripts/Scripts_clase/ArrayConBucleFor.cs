using UnityEngine;

public class ArrayConBucleFor : MonoBehaviour
{
    void Start()
    {
        // Declaramos un array de longitud 500
        int[] miArray = new int[500];

        // 1. Bucle FOR para RELLENAR el array
        for (int i = 0; i < miArray.Length; i++)
        {
            miArray[i] = i + 1;
        }

        // 2. Bucle FOR para RECORRER y mostrar cada elemento
        for (int i = 0; i < miArray.Length; i++)
        {
            Debug.Log("Índice " + i + " = " + miArray[i]);
        }
    }
}