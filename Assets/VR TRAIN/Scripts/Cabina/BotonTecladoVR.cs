using UnityEngine;

// Este script va colocado EN CADA CUBO / BOTÓN del teclado numérico
public class BotonTecladoVR : MonoBehaviour
{
    // Variables públicas para configurar en el Inspector de Unity
    public Renderer renderBoton;         // El Mesh Renderer del propio cubo del botón
    public Collider colisionadorBoton;   // El Box Collider del cubo (permite o bloquea el rayo VR)
    public Material materialNormal;      // El material original del botón (gris o negro)
    public Material materialAzul;        // El material azul cuando se pulsa

    // Start se ejecuta una sola vez al dar a PLAY
    void Start()
    {
        // Al iniciar el juego, aseguramos que el botón tenga su material normal
        renderBoton.material = materialNormal;
    }

    // Se conecta al evento 'Select Entered' o 'Activated' de ESTE cubo
    public void MarcarComoPulsado()
    {
        // Cambiamos el material del cubo al material azul
        renderBoton.material = materialAzul;
    }

    // Devuelve el botón a su estado original (material gris)
    public void ResetearMaterial()
    {
        renderBoton.material = materialNormal;
    }

    // Enciende el collider para permitir que el rayo VR haga hover y clic
    public void ActivarInteraccion()
    {
        colisionadorBoton.enabled = true;
    }

    // Apaga el collider para que el rayo VR lo ignore completamente (sin hover ni clic)
    public void DesactivarInteraccion()
    {
        colisionadorBoton.enabled = false;
    }
}