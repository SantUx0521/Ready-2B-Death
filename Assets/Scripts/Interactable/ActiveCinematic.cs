using UnityEngine;
using UnityEngine.Playables;

public class ActiveCinematic : MonoBehaviour
{
    public PlayableDirector Cinematica;
    public bool hasBeenActive;
    public void Active()
    {
        if (!hasBeenActive)
        {
            Cinematica.Play();
            hasBeenActive = true;
        }
    }
}
