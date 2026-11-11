using UnityEngine;
using UnityEngine.Rendering;

public class GunController : MonoBehaviour
{

    public GameObject bulletprefab;
    public Transform firepoint;
    public float bulletspeed = 20f;
    public float bulletlifetime = 5f;


    public AudioClip clip;
    private AudioSource source;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void Disparar()
    {
        GameObject bullet = Instantiate(bulletprefab, firepoint.position ,firepoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        source.PlayOneShot(clip);

        if (rb != null)
        {
            rb.linearVelocity = firepoint.forward * bulletspeed;
        }


        Destroy(bullet, bulletlifetime);
    }
}
