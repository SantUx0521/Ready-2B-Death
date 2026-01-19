using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class Granade : MonoBehaviour
{
    public float delay = 3f;
    public float radius = 5f;
    public float force = 700;
    public GameObject explosionEffect;
    public GameObject cameraPlayer;
    public AudioMixer audioMixer;
    public AudioSource audioSource;
    float countdown;
    public bool Exploded = false;
    void Start()
    {
        countdown = delay;
        cameraPlayer = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        if(Exploded == true) return;
        countdown -= Time.deltaTime;
        if(countdown <= 0f && Exploded == false)
        {
            Explode();
        }
    }

    public void Explode()
    {
        GameObject effect = Instantiate(explosionEffect, transform.position, transform.rotation);
        ParticleSystem ps = effect.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            Destroy(effect, ps.main.duration);
        }
        else
        {
            Destroy(effect, 2f);
        }
        Exploded = true;
        StartCoroutine(cameraPlayer.GetComponentInChildren<CameraShake>().Shake(1f, 0.07f));
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach(Collider collider in colliders)
        {
            if (collider.GetComponentInParent<Enemy>())
            {
                collider.gameObject.GetComponentInParent<Enemy>().takeDamage(50);
            }
            else if (collider.GetComponent<GameManager>())
            {
                collider.gameObject.GetComponent<GameManager>().TakeDamage(40);
            }
        }
        StartCoroutine(EnableExplosion());        
    }

    public IEnumerator EnableExplosion()
    {
        yield return new WaitForSeconds(0.001f);
        Collider[] explosionColliders = Physics.OverlapSphere(transform.position, radius);
        foreach(Collider collider in explosionColliders)
        {
            Rigidbody[] rbs = collider.GetComponents<Rigidbody>();
            foreach (Rigidbody rb in rbs)
            {
                rb.AddExplosionForce(force, transform.position, radius, 0f,ForceMode.Impulse);
            }
        }
        audioSource.Play();
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
