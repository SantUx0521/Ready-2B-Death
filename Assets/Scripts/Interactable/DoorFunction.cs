using UnityEngine;

public class DoorFunction : MonoBehaviour
{
    Animator anim;
    public GameObject doorIsSeparated;
    public string OpenName;
    public string CloseName;
    public string requiredKey;
    public bool locked;
    public GameObject padlock;
    public AudioSource audioSource;
    public AudioClip open;
    public AudioClip close;
    public AudioClip unlock;
    void Start()
    {
        if(doorIsSeparated != null)
        {
            anim = doorIsSeparated.GetComponent<Animator>();   
        }
        else
        {
            anim = GetComponent<Animator>();
        }
    }

    public void Unlock()
    {
        locked = false;
        audioSource.PlayOneShot(unlock);
    }

    public void Interact()
    {
        if(padlock != null)
        {
            if(padlock.GetComponent<Rigidbody>().isKinematic == false)
            {
                locked = false;
            }
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName(OpenName) && !locked)
            {
                anim.ResetTrigger("Open");
                anim.SetTrigger("Close");
                audioSource.PlayOneShot(close);
            }                    
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName(CloseName) && !locked)
            {
                anim.ResetTrigger("Close");
                anim.SetTrigger("Open");
                audioSource.PlayOneShot(open);
            }
    }

    public void ForceLock()
    {
        anim.SetTrigger("Close");
        audioSource.PlayOneShot(close);
        padlock = null;
        locked = true;
        requiredKey = "None";
    }
}
