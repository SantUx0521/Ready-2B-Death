using UnityEngine;

public class DoorFunction : MonoBehaviour
{
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Interact()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Door_Open"))
            {
                anim.ResetTrigger("Open");
                anim.SetTrigger("Close");
            }                    
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Door_Close"))
            {
                anim.ResetTrigger("Close");
                anim.SetTrigger("Open");
            }
    }
}
