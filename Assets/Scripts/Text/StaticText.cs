using UnityEngine;
using TMPro;
using UnityEngine.Localization;

[System.Serializable]
public class StaticText : MonoBehaviour
{
    public LocalizedString localizedText;
    public TextMeshProUGUI text;

    public void ShowText()
    {
        text.text = localizedText.GetLocalizedString();
    }
}
