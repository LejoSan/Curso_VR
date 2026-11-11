using UnityEngine;

// Este script gestiona la verificación de la clave, el bloqueo inicial y el reinicio de los botones
public class TecladoVR : MonoBehaviour
{
    // Variables de configuración de la clave
    public string codigoCorrecto = "1234";    // La contraseña esperada
    private string codigoIngresado = "";       // El texto acumulado al pulsar los números

    // Componentes visuales y de sonido
    public Renderer renderPantalla;            // El Mesh Renderer del cubo indicador sobre la pantalla
    public AudioSource audioExito;             // Sonido cuando la clave es correcta
    public AudioSource audioError;             // Sonido cuando la clave es incorrecta

    // Colores emisivos del cubo indicador
    public Color colorExito = Color.green;     // Color verde al acertar
    public Color colorError = Color.red;       // Color rojo al equivocarse

    // Arreglo con todos los botones del teclado
    public BotonTecladoVR[] todosLosBotones;

    // Integración con las Tareas (ScriptableObjects)
    public TareaSO tareaTelefonoAnterior;     // La tarea del teléfono que DEBE estar completada primero
    public TareaSO tareaPassword;             // La tarea de este teclado numérico

    // Start se ejecuta una sola vez al dar a PLAY
    void Start()
    {
        codigoIngresado = "";

        // Ocultamos el cubo indicador al iniciar
        renderPantalla.enabled = false;

        // REGLA 1: Nos suscribimos al evento de la tarea anterior (teléfono)
        tareaTelefonoAnterior.OnTareaCompletada += AlCompletarTareaAnterior;

        // Comprobamos si el teléfono ya se colgó o si debemos mantener bloqueado el teclado
        if (tareaTelefonoAnterior.estaCompletada == true)
        {
            DesbloquearTeclado();
        }
        else
        {
            BloquearTeclado();
        }
    }

    // Se activa automáticamente en el instante exacto en que la tarea del teléfono pasa a verde
    private void AlCompletarTareaAnterior(TareaSO tarea)
    {
        DesbloquearTeclado();
    }

    // Apaga la interacción de todos los botones con un bucle for
    public void BloquearTeclado()
    {
        for (int i = 0; i < todosLosBotones.Length; i = i + 1)
        {
            todosLosBotones[i].DesactivarInteraccion();
        }
    }

    // Enciende la interacción de todos los botones con un bucle for
    public void DesbloquearTeclado()
    {
        for (int i = 0; i < todosLosBotones.Length; i = i + 1)
        {
            todosLosBotones[i].ActivarInteraccion();
        }
    }

    // Se ejecuta cuando el jugador pulsa cualquier cubo de número (1 al 9)
    public void PulsarNumero(string numeroPulsado)
    {
        // Si la tarea ya se cumplió, no permitimos escribir más
        if (tareaPassword.estaCompletada == true)
        {
            return;
        }

        // Añadimos el nuevo número ingresado a la secuencia
        codigoIngresado = codigoIngresado + numeroPulsado;
    }

    // Se ejecuta al pulsar únicamente el cubo del botón OK
    public void PulsarOK()
    {
        // Si la tarea ya se cumplió, ignoramos las pulsaciones
        if (tareaPassword.estaCompletada == true)
        {
            return;
        }

        // Comprobamos si el código ingresado es el correcto
        if (codigoIngresado == codigoCorrecto)
        {
            // === CASO 1: CÓDIGO CORRECTO ===
            renderPantalla.material.SetColor("_EmissionColor", colorExito);
            renderPantalla.material.EnableKeyword("_EMISSION");
            renderPantalla.enabled = true;

            audioExito.Play();

            // Completamos la tarea en el checklist
            tareaPassword.CompletarTarea();
        }
        else
        {
            // === CASO 2: CÓDIGO INCORRECTO ===
            // Encendemos la pantalla en rojo brillante
            renderPantalla.material.SetColor("_EmissionColor", colorError);
            renderPantalla.material.EnableKeyword("_EMISSION");
            renderPantalla.enabled = true;

            audioError.Play();

            // REGLA 2: Esperamos 2 segundos en rojo y reiniciamos todo para reintentar
            Invoke(nameof(ReiniciarPantallaYBotones), 2f);
        }
    }

    // Limpia la contraseña, oculta la luz roja y devuelve todos los botones a su material gris
    public void ReiniciarPantallaYBotones()
    {
        codigoIngresado = "";

        // Ocultamos el cubo indicador (vuelve a verse la textura base de la pantalla)
        renderPantalla.enabled = false;

        // Recorremos la lista de botones y devolvemos cada uno a su color original
        for (int i = 0; i < todosLosBotones.Length; i = i + 1)
        {
            todosLosBotones[i].ResetearMaterial();
        }
    }

    // Cancelamos la suscripción al evento si el objeto se desactiva
    private void OnDisable()
    {
        tareaTelefonoAnterior.OnTareaCompletada -= AlCompletarTareaAnterior;
    }
}