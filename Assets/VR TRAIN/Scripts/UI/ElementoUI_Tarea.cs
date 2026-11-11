using UnityEngine;
using TMPro;
using UnityEngine.UI;

// Este script va en cada fila de tarea dentro del Canvas de la UI
public class ElementoUI_Tarea : MonoBehaviour
{
    // Variables públicas para configurar en el Inspector de Unity
    public TareaSO tareaAsociada;            // El archivo ScriptableObject de esta tarea
    public TextMeshProUGUI textoDescripcion; // El componente de texto de esta fila
    public Toggle casillaCheck;              // La casilla de verificación (check) de esta fila
    public GameObject siguienteFilaTarea;   // La siguiente fila de UI que se encenderá al completar esta

    // Colores visuales del texto
    public Color colorPendiente = Color.white;
    public Color colorCompletado = Color.green;


    // OnEnable se ejecuta automáticamente cada vez que esta fila se activa en pantalla
    private void OnEnable()
    {
        // Verificamos que tengamos un ScriptableObject asignado en el Inspector
        if (tareaAsociada != null)
        {
            // Nos suscribimos al evento: "Avísame cuando la tarea se complete"
            tareaAsociada.OnTareaCompletada += ActualizarGraficos;

            // Actualizamos la apariencia por si la tarea ya estaba completada
            ActualizarGraficos(tareaAsociada);
        }
    }

    // OnDisable se ejecuta automáticamente si la fila se desactiva
    private void OnDisable()
    {
        if (tareaAsociada != null)
        {
            // Cancelamos la suscripción para evitar usar memoria innecesaria
            tareaAsociada.OnTareaCompletada -= ActualizarGraficos;
        }
    }

    // Esta función se ejecuta automáticamente cuando el ScriptableObject notifica un cambio
    private void ActualizarGraficos(TareaSO tarea)
    {
        // 1. Escribimos la descripción de la tarea y cambiamos el color del texto
        if (textoDescripcion != null)
        {
            textoDescripcion.text = tarea.descripcion;

            // Si la tarea se ha completado, pintamos en verde; si no, en blanco
            if (tarea.estaCompletada == true)
            {
                textoDescripcion.color = colorCompletado;
            }
            else
            {
                textoDescripcion.color = colorPendiente;
            }
        }

        // 2. Marcamos o desmarcamos la casilla del check
        if (casillaCheck != null)
        {
            casillaCheck.isOn = tarea.estaCompletada;
        }

        // 3. SECUENCIA AUTOMÁTICA: Si esta tarea se completó, encendemos la siguiente fila en la UI
        if (tarea.estaCompletada == true)
        {
            if (siguienteFilaTarea != null)
            {
                siguienteFilaTarea.SetActive(true); // Enciende la siguiente tarea en el Checklist
            }
        }
    }
}