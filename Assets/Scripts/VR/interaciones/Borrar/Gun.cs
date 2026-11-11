using UnityEngine;

public class Gun : MonoBehaviour
{
    // Configuración del Proyectil
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 20f;
    public float bulletLifetime = 5f; // Segundos antes de destruir la bala

    // Configuración de Audio
    public AudioClip clip;
    private AudioSource source;

    private void Start()
    {
        // Obtiene la referencia del componente de audio adjunto al arma
        source = GetComponent<AudioSource>();
    }

    public void FireBullet()
    {
    
        // 1. Creación del objeto en la escena en la posición/rotación del cañón
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        // 2. Reproducción del sonido de disparo
        source.PlayOneShot(clip);

        // 3. Aplicar velocidad física hacia el frente (Sintaxis oficial de Unity 6)
        if (rb != null)
        {
            rb.linearVelocity = firePoint.forward * bulletSpeed;
        }

        // 4. Limpieza de memoria (destruye la bala tras X segundos)
        Destroy(bullet, bulletLifetime);
    }
}