using UnityEngine;

public class ShowAnyText : MonoBehaviour, InteractInterface
{
    [SerializeField] GameObject canvas;
    private GameManager gameManager;
    private StaticText text;

    public void Interact()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        text = GetComponent<StaticText>();
        text.ShowText();
        canvas.SetActive(true);
        gameManager.HUD.SetActive(false);
        gameManager.GetActiveCanva(canvas);
        gameManager.Pause();
        gameManager.isMenuOn = true;
    }
}
