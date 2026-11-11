using PixeLadder.EasyTransition;
using UnityEngine;

// Este script controla el resalte de la puerta al apuntarla en VR y cambia de escena al hacer clic
public class PuertaTransicionVR : MonoBehaviour
{
    // Variables públicas para arrastrar desde el Inspector de Unity
    public Outline efectoOutline;                       // El componente de borde brillante del modelo 3D
    public string nombreEscenaDestino = "Train_2";  // Nombre exacto de la escena a la que vamos a ir
    public TransitionEffect estiloTransicion;           // El archivo de efecto visual (Fade, Slide, etc.)

    // Start se ejecuta automáticamente una sola vez al dar a PLAY
    void Start()
    {
        // Nos aseguramos de que el borde brillante empiece apagado
        efectoOutline.enabled = false;
    }

    // Se conecta al evento 'Hover Entered' (cuando el rayo del mando VR apunta a la puerta)
    public void MostrarResalte()
    {
        efectoOutline.enabled = true; // Enciende el contorno brillante
    }

    // Se conecta al evento 'Hover Exited' (cuando el rayo deja de apuntar a la puerta)
    public void OcultarResalte()
    {
        efectoOutline.enabled = false; // Apaga el contorno brillante
    }

    // Se conecta al evento 'Select' o 'Activate' (al pulsar el gatillo sobre la puerta)
    public void CargarNuevaEscena()
    {
        efectoOutline.enabled = false; // Apagamos el contorno antes de cambiar de pantalla

        // Le ordena al gestor de transiciones cargar la nueva escena usando el efecto elegido
        SceneTransitioner.Instance.LoadScene(nombreEscenaDestino, estiloTransicion);
    }
}