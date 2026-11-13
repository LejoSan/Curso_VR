using UnityEngine;
using UnityEngine.UI;

public class VRSliderObjectController
    : MonoBehaviour
{
    [Header("Referencia UI")]
    [SerializeField] private Scrollbar miScrollbar;

    [Header("Tus 3 Objetos 3D (Orden: 0, 0.5, 1)")]
    [SerializeField] private GameObject objetoPosicion0;
    [SerializeField] private GameObject objetoPosicion05;
    [SerializeField] private GameObject objetoPosicion1;

    private int indiceActual = -1;

    private void Start()
    {
        // Conecta automáticamente el evento OnValueChanged del Scrollbar
        if (miScrollbar != null)
        {
            miScrollbar.onValueChanged.AddListener(AlCambiarValorScrollbar);
            // Sincroniza los objetos con el valor inicial (0) al arrancar
            AlCambiarValorScrollbar(miScrollbar.value);
        }
    }

    public void AlCambiarValorScrollbar(float valor)
    {
        int nuevoIndice;

        if (valor < 0.33f)
        {
            nuevoIndice = 0; // Corresponde a 0
        }
        else if (valor < 0.67f)
        {
            nuevoIndice = 1; // Corresponde a 0.5
        }
        else
        {
            nuevoIndice = 2; // Corresponde a 1
        }

        // Si seguimos en el mismo tramo, no hacemos nada
        if (nuevoIndice == indiceActual) return;
        indiceActual = nuevoIndice;

        // Activa solo el objeto correspondiente y oculta los demás
        if (objetoPosicion0 != null) objetoPosicion0.SetActive(nuevoIndice == 0);
        if (objetoPosicion05 != null) objetoPosicion05.SetActive(nuevoIndice == 1);
        if (objetoPosicion1 != null) objetoPosicion1.SetActive(nuevoIndice == 2);
    }

    private void OnDestroy()
    {
        if (miScrollbar != null)
        {
            miScrollbar.onValueChanged.RemoveListener(AlCambiarValorScrollbar);
        }
    }
}