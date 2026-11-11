using UnityEngine;

// Este script controla los botones físicos de abrir y cerrar puertas, ejecuta sus sonidos y animaciones,
// y guarda la variable 'puertasCerradas' para la comprobación de seguridad del tren.
public class ControladorPuertaVR : MonoBehaviour
{
    // Componentes 3D de las puertas
    public Animator animadorPuerta;            // El Animator con las animaciones 'Abrir' y 'Cerrar'
    public AudioSource audioAbrirPuerta;        // Altavoz 3D con el sonido de apertura
    public AudioSource audioCerrarPuerta;       // Altavoz 3D con el sonido de cierre

    // Botones físicos de la consola
    public Renderer renderBotonAbrir;           // Mesh Renderer del botón ABRIR
    public Renderer renderBotonCerrar;          // Mesh Renderer del botón CERRAR
    public Collider colisionadorBotonAbrir;     // Box Collider del botón ABRIR
    public Collider colisionadorBotonCerrar;    // Box Collider del botón CERRAR

    // Materiales visuales de encendido / apagado
    public Material materialNormal;             // Material base (botón inactivo)
    public Material materialActivo;             // Material iluminado (botón activo)

    // VARIABLE CLAVE PARA LA FASE 5 (PALANCA DEL TREN)
    public bool puertasCerradas = false;        // true = cerradas / false = abiertas (impide acelerar si está en false)

    // Integración con el Checklist (ScriptableObjects)
    public TareaSO tareaTecladoAnterior;       // Tarea del teclado que DEBE completarse antes
    public TareaSO tareaCerrarPuertas;          // TareaSO correspondiente a esta cuarta fase

    // ==============================================================================================
    // [ESCALABILIDAD FUTURA - CÁMARA Y MONITOR DE SEGURIDAD]
    // Descomentar estas líneas únicamente cuando desees añadir la pantalla 3D en tiempo real:
    // public GameObject camaraPuerta;          // Cámara 3D exterior
    // public GameObject monitorRawImage;       // Canvas 3D con la Render Texture en la cabina
    // public float duracionAnimacion = 3.0f;   // Tiempo de espera antes de apagar la pantalla
    // ==============================================================================================

    // Start se ejecuta una sola vez al dar a PLAY
    void Start()
    {
        // Al arrancar, las puertas se consideran abiertas
        puertasCerradas = false;

        // Iluminamos el botón de ABRIR para reflejar que la puerta está abierta en la escena
        renderBotonAbrir.material = materialActivo;
        renderBotonCerrar.material = materialNormal;

        // REGLA DE SECUENCIA: Nos suscribimos a la tarea del teclado
        tareaTecladoAnterior.OnTareaCompletada += AlCompletarTecladoAnterior;

        // Si la contraseña ya fue introducida, activamos los botones; si no, los mantenemos bloqueados
        if (tareaTecladoAnterior.estaCompletada == true)
        {
            DesbloquearBotones();
        }
        else
        {
            BloquearBotones();
        }
    }

    // Se activa automáticamente cuando la tarea del teclado pasa a verde en la UI
    private void AlCompletarTecladoAnterior(TareaSO tarea)
    {
        DesbloquearBotones();
    }

    // Apaga los Box Colliders para evitar que el rayo VR detecte o pulse los botones
    public void BloquearBotones()
    {
        colisionadorBotonAbrir.enabled = false;
        colisionadorBotonCerrar.enabled = false;
    }

    // Enciende los Box Colliders para permitir la interacción VR
    public void DesbloquearBotones()
    {
        colisionadorBotonAbrir.enabled = true;
        colisionadorBotonCerrar.enabled = true;
    }

    // Se conecta al evento 'Select Entered' o 'Activated' del botón ABRIR
    public void PulsarBotonAbrirPuertas()
    {
        // Si las puertas ya están abiertas, no hace nada
        if (puertasCerradas == false)
        {
            return;
        }

        // 1. Cambiamos el estado de seguridad a ABIERTO
        puertasCerradas = false;

        // 2. Cambiamos las luces de los botones (ABRIR encendido, CERRAR apagado)
        renderBotonAbrir.material = materialActivo;
        renderBotonCerrar.material = materialNormal;

        // 3. Suena el audio de apertura
        audioAbrirPuerta.Play();

        // 4. Ejecutamos el Trigger "Abrir" en el Animator
        animadorPuerta.SetTrigger("Abrir");

        // [FUTURO - CÁMARA]: camaraPuerta.SetActive(true); monitorRawImage.SetActive(true);
        // [FUTURO - APAGADO]: Invoke(nameof(ApagarPantalla), duracionAnimacion + 1.0f);
    }

    // Se conecta al evento 'Select Entered' o 'Activated' del botón CERRAR
    public void PulsarBotonCerrarPuertas()
    {
        // Si las puertas ya están cerradas, no hace nada
        if (puertasCerradas == true)
        {
            return;
        }

        // 1. Cambiamos el estado de seguridad a CERRADO
        puertasCerradas = true;

        // 2. Cambiamos las luces de los botones (ABRIR apagado, CERRAR encendido)
        renderBotonAbrir.material = materialNormal;
        renderBotonCerrar.material = materialActivo;

        // 3. Suena el audio de cierre
        audioCerrarPuerta.Play();

        // 4. Ejecutamos el Trigger "Cerrar" en el Animator
        animadorPuerta.SetTrigger("Cerrar");

        // 5. Marcamos la tarea como completada en el Checklist (pasa a verde)
        tareaCerrarPuertas.CompletarTarea();

        // [FUTURO - CÁMARA]: camaraPuerta.SetActive(true); monitorRawImage.SetActive(true);
        // [FUTURO - APAGADO]: Invoke(nameof(ApagarPantalla), duracionAnimacion + 1.0f);
    }

    // Cancelamos la suscripción al evento si el objeto se desactiva en Unity
    private void OnDisable()
    {
        tareaTecladoAnterior.OnTareaCompletada -= AlCompletarTecladoAnterior;
    }
}