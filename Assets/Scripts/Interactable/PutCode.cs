using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PutCode : MonoBehaviour
{
    public TMP_InputField input;
    [SerializeField] private string code;

    public void ActivateChar(string chara)
    {
        input.text += chara;
        input.ActivateInputField();
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
        }
        else
        {
            Debug.Log("Mal perra");
        }
    }
}
