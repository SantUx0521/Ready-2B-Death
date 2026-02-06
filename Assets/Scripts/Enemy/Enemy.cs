using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Enemy : MonoBehaviour
{
    public float health = 30;
    public int damage = 10;
    public bool isDead = false;
    public AudioClip scream;
    public AudioSource audioSource;
    public GameObject blood;
    public GameObject granade;
    public string tipe;
    public GameObject Bullet;
    RagdollEnemy ragdoll;
    bool already = false;
    RigBuilder rig;

    public PlayerController player;

    void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        ragdoll = GetComponent<RagdollEnemy>();
        rig = GetComponentInChildren<RigBuilder>();
    }

    void Update()
    {
        die();
        if(!isDead){return;}
    }

    public void takeDamage(float damage){
        health -= damage;
    }
    public IEnumerator Bleed(Vector3 dir, Vector3 hitPoint)
    {
        blood.SetActive(true);
        blood.transform.position = hitPoint;
        Quaternion targetRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(
                blood.transform.rotation,
                targetRot,
                Time.deltaTime * 8f
            );            
        yield return new WaitForSeconds(1f);
        blood.SetActive(false);
    }

    public void headShot()
    {
        health = 0;
    }

    void die()
    {
        if(health <= 0 && !already)
        {
            isDead = true;
            StartCoroutine(DeathConsecuences());
            rig.enabled = false;
            ragdoll.ragdollON();
            Destroy(gameObject, 120);
        }   
    }

    private IEnumerator DeathConsecuences()
    {
        if (!already)
        {
            audioSource.PlayOneShot(scream);
            Instantiate(Bullet, transform.position, transform.rotation);
            yield return null;
            already = true;
        }
    }
}
