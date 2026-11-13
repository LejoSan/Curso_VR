using UnityEngine;
using UnityEngine.InputSystem;

public class StarWarsShipController : MonoBehaviour
{
    [Header("1. Controles de los Mandos VR")]
    // Aquí conectarás en Unity el joystick izquierdo (solo izquierda y derecha)
    public InputActionProperty joystickIzquierdo;
    // Aquí conectarás en Unity el joystick derecho (apuntar y girar la nave)
    public InputActionProperty joystickDerecho;

    [Header("2. Velocidades de Vuelo")]
    // Velocidad fija a la que la nave avanzará sola todo el tiempo
    public float velocidadCruceroConstante = 35f;
    // Velocidad al esquivar hacia la izquierda o derecha con el joystick izquierdo
    public float velocidadLateral = 20f;
    // Rapidez con la que la nave gira hacia los lados
    public float velocidadGiroHorizontal = 45f;
    // Rapidez con la que la nave sube o baja el morro
    public float velocidadInclinacionVertical = 35f;

    [Header("3. Suavizado para evitar mareos")]
    // Controla qué tan suave arranca la nave y qué tan suave esquiva a los lados
    public float suavizadoMovimiento = 3.0f;
    // Controla cuánto tarda la nave en empezar y terminar de girar
    public float suavizadoGiro = 3.0f;

    [Header("4. Inclinación visual de la nave (Alabeo)")]
    // Aquí debes arrastrar SOLO el modelo 3D de la nave (no el XR Origin)
    public Transform modeloVisualNave;
    // Cuántos grados máximos se tumba la nave al girar a tope
    public float anguloMaximoInclinacion = 25f;
    // Qué tan rápido se tumba y vuelve a enderezarse el modelo 3D
    public float suavizadoInclinacion = 4.0f;

    // --- VARIABLES INTERNAS (La memoria de la nave) ---
    // Guardan la velocidad y rotación actuales para ir cambiándolas poco a poco
    private Vector3 velocidadActual;
    private Vector2 giroActual;
    private float inclinacionActualZ;


    // Se ejecuta automáticamente cuando la nave se activa en la escena
    private void OnEnable()
    {
        // Encendemos la escucha de ambos joysticks para que Unity lea tus dedos
        joystickIzquierdo.action.Enable();
        joystickDerecho.action.Enable();
    }

    // Se ejecuta automáticamente cuando la nave se desactiva o se destruye
    private void OnDisable()
    {
        // Apagamos la escucha de los joysticks para ahorrar recursos
        joystickIzquierdo.action.Disable();
        joystickDerecho.action.Disable();
    }


    // Se ejecuta en cada fotograma del juego (unas 72 o 90 veces por segundo en VR)
    private void Update()
    {
        // =================================================================
        // PASO 1: LEER LOS JOYSTICKS
        // =================================================================
        // Obtenemos un valor entre -1 y 1 de ambos joysticks
        Vector2 lecturaJoystickIzquierdo = joystickIzquierdo.action.ReadValue<Vector2>();
        Vector2 lecturaJoystickDerecho = joystickDerecho.action.ReadValue<Vector2>();


        // =================================================================
        // PASO 2: AVANCE AUTOMÁTICO + ESQUIVE LATERAL SUAVE
        // =================================================================
        // Usamos ÚNICAMENTE el eje X (izquierda/derecha) del joystick izquierdo.
        // Ignoramos por completo lecturaJoystickIzquierdo.y
        float velocidadDeseadaX = lecturaJoystickIzquierdo.x * velocidadLateral;

        // No subimos ni bajamos verticalmente como un ascensor
        float velocidadDeseadaY = 0f;

        // En el eje Z (hacia adelante), ponemos siempre la velocidad constante
        float velocidadDeseadaZ = velocidadCruceroConstante;

        // Juntamos esas tres direcciones en un solo vector de velocidad objetivo
        Vector3 velocidadObjetivo = new Vector3(velocidadDeseadaX, velocidadDeseadaY, velocidadDeseadaZ);

        // Acercamos la velocidad actual a la objetivo poco a poco para evitar tirones al arrancar o esquivar
        float pasoMovimiento = Time.deltaTime * suavizadoMovimiento;
        velocidadActual = Vector3.Lerp(velocidadActual, velocidadObjetivo, pasoMovimiento);

        // Movemos el objeto padre de la nave usando esa velocidad suavizada
        Vector3 desplazamientoEsteFotograma = velocidadActual * Time.deltaTime;
        transform.Translate(desplazamientoEsteFotograma, Space.Self);


        // =================================================================
        // PASO 3: GIRO DE LA NAVE (IZQUIERDA, DERECHA, ARRIBA, ABAJO)
        // =================================================================
        // Suavizamos la entrada del joystick derecho para que los giros no den tirones
        float pasoGiro = Time.deltaTime * suavizadoGiro;
        giroActual = Vector2.Lerp(giroActual, lecturaJoystickDerecho, pasoGiro);

        // Calculamos cuántos grados giramos en este fotograma
        float giroHorizontalYaw = giroActual.x * velocidadGiroHorizontal * Time.deltaTime;
        // Ponemos el signo menos (-) para que al empujar hacia adelante el morro baje (como un avión real)
        float giroVerticalPitch = -giroActual.y * velocidadInclinacionVertical * Time.deltaTime;

        // Aplicamos la rotación al objeto padre (el eje Z lo dejamos en 0 para no marear al jugador)
        transform.Rotate(giroVerticalPitch, giroHorizontalYaw, 0f, Space.Self);


        // =================================================================
        // PASO 4: MANTENER EL HORIZONTE ESTABLE (ANTI-MAREO)
        // =================================================================
        // Enderezamos suavemente el eje Z del padre para que tu cabeza siempre esté nivelada
        Vector3 rotacionActualEuler = transform.eulerAngles;
        float ejeZEnderezado = Mathf.LerpAngle(rotacionActualEuler.z, 0f, Time.deltaTime * 2f);
        transform.rotation = Quaternion.Euler(rotacionActualEuler.x, rotacionActualEuler.y, ejeZEnderezado);


        // =================================================================
        // PASO 5: INCLINACIÓN VISUAL DEL MODELO 3D (BANKING)
        // =================================================================
        // Si giras a la derecha (giroActual.x positivo), inclinamos la nave hacia la derecha (Z negativo)
        float inclinacionDeseadaZ = -giroActual.x * anguloMaximoInclinacion;

        // Suavizamos esa inclinación para que la nave se tumbe con elegancia
        float pasoInclinacion = Time.deltaTime * suavizadoInclinacion;
        inclinacionActualZ = Mathf.Lerp(inclinacionActualZ, inclinacionDeseadaZ, pasoInclinacion);

        // Aplicamos esa inclinación ÚNICAMENTE al modelo 3D de la nave, dejando tu cabeza (XR Origin) quieta
        modeloVisualNave.localRotation = Quaternion.Euler(0f, 0f, inclinacionActualZ);
    }
}