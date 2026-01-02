using UnityEngine;
using UnityEngine.InputSystem;

public class CameraShake : MonoBehaviour
{
    PlayerInput playerInput;
    Animator anim;

    void Start()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Shake();
    }

    void Shake()
    {
        if(playerInput.actions["Move"].ReadValue<Vector2>() != Vector2.zero)
        {
            anim.SetBool("Walking", true);
        }
        else
        {
            anim.SetBool("Walking", false);
        }
    }
}
