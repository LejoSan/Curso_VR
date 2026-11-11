using PixeLadder.EasyTransition;
using UnityEngine;

// Este script controla el movimiento progresivo del tren, la dirección de avance y la transición al final del recorrido.
public class ControladorTrenVR : MonoBehaviour
{
    [Header("Ajustes de Velocidad")]
    public float velocidadObjetivo = 0.0f;       // Velocidad a la que quiere ir el tren
    public float velocidadActual = 0.0f;         // Velocidad real suavizada
    public float aceleracion = 2.0f;              // Qué tan rápido acelera/frena (unidades por segundo)

    public float velocidadLenta = 2.0f;          // Marcha 1 (Media)
    public float velocidadRapida = 5.0f;         // Marcha 2 (Máxima)

    [Header("Dirección de Avance")]
    public Vector3 direccionAvance = Vector3.forward; // Cambia en el Inspector si avanza hacia otro eje (ej: Vector3.back o Vector3.right)

    public int posicionPalanca = 0;

    [Header("Referencias de Cabina")]
    public ControladorPuertaVR controladorPuerta;
    public AudioSource audioAlarmaPuerta;
    public AudioSource audioMotorTren;

    [Header("Transición de Escena")]
    public string nombreEscenaDestino = "Escena_03";
    public TransitionEffect estiloTransicion;

    public TareaSO tareaAcelerarTren;

    void Start()
    {
        posicionPalanca = 0;
        velocidadObjetivo = 0.0f;
        velocidadActual = 0.0f;
    }

    void Update()
    {
        // 1. BLOQUEO DE SEGURIDAD: Si las puertas se abren con el tren andando
        if (posicionPalanca > 0 && controladorPuerta != null && controladorPuerta.puertasCerradas == false)
        {
            posicionPalanca = 0;
            velocidadObjetivo = 0.0f;

            if (audioAlarmaPuerta != null && audioAlarmaPuerta.isPlaying == false)
            {
                audioAlarmaPuerta.Play();
            }

            if (audioMotorTren != null)
            {
                audioMotorTren.Stop();
            }
        }

        // 2. ACELERACIÓN SUAVE: Transición progresiva hacia la velocidad objetivo
        velocidadActual = Mathf.MoveTowards(velocidadActual, velocidadObjetivo, aceleracion * Time.deltaTime);

        // 3. DESPLAZAMIENTO: Mueve el tren en la dirección configurada
        if (velocidadActual > 0.01f)
        {
            transform.Translate(direccionAvance * velocidadActual * Time.deltaTime);
        }
    }

    // Invocado por PalancaVR al cambiar de tope
    public void CambiarPosicionPalanca(int nuevaPosicion)
    {
        posicionPalanca = nuevaPosicion;

        // MARCHA 0: DETENIDO
        if (nuevaPosicion == 0)
        {
            velocidadObjetivo = 0.0f;

            if (audioMotorTren != null)
            {
                audioMotorTren.Stop();
            }
        }

        // MARCHA 1: VELOCIDAD MEDIA
        if (nuevaPosicion == 1)
        {
            if (controladorPuerta != null && controladorPuerta.puertasCerradas == true)
            {
                velocidadObjetivo = velocidadLenta;

                if (audioMotorTren != null && audioMotorTren.isPlaying == false)
                {
                    audioMotorTren.Play();
                }

                if (tareaAcelerarTren != null)
                {
                    tareaAcelerarTren.CompletarTarea();
                }
            }
            else
            {
                posicionPalanca = 0;
                velocidadObjetivo = 0.0f;

                if (audioAlarmaPuerta != null && audioAlarmaPuerta.isPlaying == false)
                {
                    audioAlarmaPuerta.Play();
                }
            }
        }

        // MARCHA 2: VELOCIDAD MÁXIMA
        if (nuevaPosicion == 2)
        {
            if (controladorPuerta != null && controladorPuerta.puertasCerradas == true)
            {
                velocidadObjetivo = velocidadRapida;

                if (audioMotorTren != null && audioMotorTren.isPlaying == false)
                {
                    audioMotorTren.Play();
                }

                if (tareaAcelerarTren != null)
                {
                    tareaAcelerarTren.CompletarTarea();
                }
            }
            else
            {
                posicionPalanca = 0;
                velocidadObjetivo = 0.0f;

                if (audioAlarmaPuerta != null && audioAlarmaPuerta.isPlaying == false)
                {
                    audioAlarmaPuerta.Play();
                }
            }
        }
    }

    // Detecta la llegada al final del trayecto
    private void OnTriggerEnter(Collider otroObjeto)
    {
        if (otroObjeto.CompareTag("FinalVia") == true)
        {
            SceneTransitioner.Instance.LoadScene(nombreEscenaDestino, estiloTransicion);
        }
    }
}