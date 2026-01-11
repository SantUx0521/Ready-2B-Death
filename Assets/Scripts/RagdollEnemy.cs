using UnityEngine;

public class RagdollEnemy : MonoBehaviour
{
    Animator anim;
    Rigidbody[] rigidbodyCollider;
    void Start()
    {
        anim = GetComponent<Animator>();
        ragdollParts();
        ragdollOFF();
    }

    public void ragdollON()
    {
        anim.enabled = false;
        foreach (Rigidbody rb in rigidbodyCollider)
        {
            rb.isKinematic = false;
        }
    }

    void ragdollOFF()
    {
        foreach (Rigidbody rb in rigidbodyCollider)
        {
            rb.isKinematic = true;
        }
        anim.enabled = true; 
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Veremos si lo uso en un futuro, de momento manejare unicamente ragdolls cuando muera
    }

    void ragdollParts()
    {
        rigidbodyCollider = GetComponentsInChildren<Rigidbody>();
    }
}
