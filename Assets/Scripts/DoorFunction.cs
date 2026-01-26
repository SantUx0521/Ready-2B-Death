using UnityEngine;

public class DoorFunction : MonoBehaviour
{
    Animator anim;
    public string OpenName;
    public string CloseName;
    public string requiredKey;
    public bool locked;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Unlock()
    {
        locked = false;
    }

    public void Interact()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(OpenName) && !locked)
            {
                anim.ResetTrigger("Open");
                anim.SetTrigger("Close");
            }                    
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName(CloseName) && !locked)
            {
                anim.ResetTrigger("Close");
                anim.SetTrigger("Open");
            }
    }
}
