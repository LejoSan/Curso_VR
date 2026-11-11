using UnityEngine;

// Este script controla el paso a paso de las pantallas de menú antes de empezar la simulación VR
public class MenuInicialVR : MonoBehaviour
{
    // Variables públicas para arrastrar desde el Inspector de Unity
    public GameObject panelJugar;        // Pantalla 1: Botón principal de Jugar
    public GameObject panelObjetivos;    // Pantalla 2: Explicación de los objetivos
    public GameObject panelControles;    // Pantalla 3: Instrucciones de los mandos
    public GameObject sistemaLocomotion; // Objeto que permite al jugador mover la cámara/joysticks

    // Start se ejecuta automáticamente una sola vez cuando le damos a PLAY en Unity
    void Start()
    {
        // 1. Encendemos únicamente la primera pantalla al arrancar
        panelJugar.SetActive(true);
        panelObjetivos.SetActive(false);
        panelControles.SetActive(false);

        // 2. Apagamos el movimiento del personaje para que no se mueva mientras lee el menú
        sistemaLocomotion.SetActive(false);
    }

    // Se conecta al botón "JUGAR" de la Pantalla 1
    public void BotonJugarPulsado()
    {
        panelJugar.SetActive(false);     // Apaga la pantalla 1
        panelObjetivos.SetActive(true);  // Enciende la pantalla 2
    }

    // Se conecta al botón "CONTINUAR" de la Pantalla 2 (Objetivos)
    public void BotonObjetivosContinuarPulsado()
    {
        panelObjetivos.SetActive(false); // Apaga la pantalla 2
        panelControles.SetActive(true);  // Enciende la pantalla 3
    }

    // Se conecta al botón "COMENZAR SIMULACIÓN" de la Pantalla 3 (Controles)
    public void BotonComenzarSimulacionPulsado()
    {
        panelControles.SetActive(false);  // Apaga la pantalla 3 (Cierra el menú)
        sistemaLocomotion.SetActive(true); // ¡Activa los joysticks para poder caminar en VR!
    }
}