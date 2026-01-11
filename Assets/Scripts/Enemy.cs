using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Enemy : MonoBehaviour
{
    public int health = 30;
    public int damage = 10;
    public bool isDead = false;
    RagdollEnemy ragdoll;
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

    public void takeDamage(int damage){
        health -= damage;
    }

    public void headShot()
    {
        health = 0;
    }

    void die()
    {
        if(health <= 0)
        {
            isDead = true;
            rig.enabled = false;
            ragdoll.ragdollON();
        }
    }
}
