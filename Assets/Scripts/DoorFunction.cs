using UnityEngine;

public class DoorFunction : MonoBehaviour
{
    Animator anim;
    public string OpenName;
    public string CloseName;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Interact()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(OpenName))
            {
                anim.ResetTrigger("Open");
                anim.SetTrigger("Close");
            }                    
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName(CloseName))
            {
                anim.ResetTrigger("Close");
                anim.SetTrigger("Open");
            }
    }
}
