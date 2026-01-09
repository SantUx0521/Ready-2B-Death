using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 30;
    private bool isDead = false;

    public PlayerController player;

    void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
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
            Destroy(gameObject);
        }
    }
}
