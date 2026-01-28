using UnityEngine;
using UnityEngine.Playables;

public class ActiveCinematic : MonoBehaviour, InteractInterface
{
    public PlayableDirector Cinematica;
    public bool hasBeenActive;
    public void Interact()
    {
        if (!hasBeenActive)
        {
            Cinematica.Play();
            hasBeenActive = true;
        }
    }
}
