using UnityEngine;

// Este script controla la lógica del teléfono VR (audio 3D, activación de la UI y completar la tarea)
public class ControladorTelefonoVR : MonoBehaviour
{
    // Variables públicas para arrastrar desde el Inspector de Unity
    public AudioSource audioInstrucciones;   // El altavoz 3D ubicado en el auricular
    public GameObject canvasChecklistVR;     // El Canvas flotante en la cabina con la lista de tareas
    public TareaSO tareaDescolgarTelefono;  // El archivo de datos (ScriptableObject) de esta tarea

    // Variables internas para controlar el estado del teléfono
    private bool uiActivada = false;         // Permite encender el Canvas solo la primera vez que se descuelga
    private bool haSidoDescolgado = false;   // Evita que la tarea se complete sola al iniciar el juego en la base

    // Start se ejecuta automáticamente una sola vez al dar a PLAY
    void Start()
    {
        // Ocultamos la pantalla de tareas al iniciar la escena
        canvasChecklistVR.SetActive(false);
    }

    // Se conecta al evento 'Select Exited' del XR Socket Interactor (al retirar el auricular de la base)
    public void AlDescolgarTelefono()
    {
        haSidoDescolgado = true; // Registramos que el jugador ha cogido el teléfono con la mano

        // 1. Mostramos el panel de tareas en la cabina la primera vez
        if (!uiActivada)
        {
            canvasChecklistVR.SetActive(true);
            uiActivada = true;
        }

        // 2. Reproducimos las instrucciones de voz en el auricular
        audioInstrucciones.Play();
    }

    // Se conecta al evento 'Select Entered' del XR Socket Interactor (al soltar el auricular en la base)
    public void AlColgarTelefono()
    {
        // Si el teléfono no se ha descolgado con la mano primero, ignoramos el evento de arranque
        if (!haSidoDescolgado) return;

        // 1. Detenemos la voz del auricular
        audioInstrucciones.Stop();

        // 2. Notificamos al ScriptableObject que la tarea finalizó (la UI cambiará el texto a verde automáticamente)
        tareaDescolgarTelefono.CompletarTarea();
    }
}