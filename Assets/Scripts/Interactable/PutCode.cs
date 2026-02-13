using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PutCode : MonoBehaviour, InteractInterface
{
    public GameObject canvas;
    private GameManager gameManager;
    public TMP_InputField input;
    [SerializeField] private string code;

    public void ActivateChar(string chara)
    {
        if(input.text.Length < input.characterLimit)
        {
            input.text += chara;
            input.ActivateInputField();
        } 
    }

    public void Interact()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        canvas.SetActive(true);
        gameManager.HUD.SetActive(false);
        gameManager.GetActiveCanva(canvas);
        gameManager.Pause();
        gameManager.isMenuOn = true;
    }

    public void Delete()
    {
        if(input.text.Length >= 0)
        {
            input.text = input.text.Substring(0, input.text.Length - 1);
        }
    }

    public void ValidCode()
    {
        if(input.text == code)
        {
            GetComponent<DoorFunction>().Unlock();
            gameManager.Resume();
            GetComponent<DoorFunction>().Interact();
        }
    }
}
