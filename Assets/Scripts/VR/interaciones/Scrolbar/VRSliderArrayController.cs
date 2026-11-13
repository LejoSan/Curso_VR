using UnityEngine;
using UnityEngine.UI;

public class VRSliderArrayController : MonoBehaviour
{
    public Scrollbar miScrollbar;
    public GameObject[] modelos3D;

    private int indiceActual = -1;

    private void Start()
    {
        if (miScrollbar != null)
        {
            miScrollbar.onValueChanged.AddListener(AlCambiarValorScrollbar);
            AlCambiarValorScrollbar(miScrollbar.value);
        }
    }

    public void AlCambiarValorScrollbar(float valorScroll)
    {
        // Si no hay modelos asignados en el array, salimos para evitar errores
        if (modelos3D == null || modelos3D.Length == 0)
        {
            return;
        }

        // Calculamos el índice correspondiente según la cantidad total de modelos
        int totalModelos = modelos3D.Length;
        int nuevoIndice = Mathf.FloorToInt(valorScroll * totalModelos);

        // Cuando el scroll llega exactamente a 1.0, el cálculo da igual al total.
        // Con este if nos aseguramos de que no se salga del límite del array.
        if (nuevoIndice >= totalModelos)
        {
            nuevoIndice = totalModelos - 1;
        }

        if (nuevoIndice < 0)
        {
            nuevoIndice = 0;
        }

        // Si seguimos en el mismo modelo, no hacemos nada
        if (nuevoIndice == indiceActual)
        {
            return;
        }

        indiceActual = nuevoIndice;

        // Recorremos todo el array: encendemos solo el elegido y apagamos los demás
        for (int i = 0; i < modelos3D.Length; i++)
        {
            if (modelos3D[i] != null)
            {
                if (i == indiceActual)
                {
                    modelos3D[i].SetActive(true);
                }
                else
                {
                    modelos3D[i].SetActive(false);
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (miScrollbar != null)
        {
            miScrollbar.onValueChanged.RemoveListener(AlCambiarValorScrollbar);
        }
    }
}