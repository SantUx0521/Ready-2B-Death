using UnityEngine;
using TMPro;
using UnityEngine.Localization;

public class PauseLocalization : MonoBehaviour
{
    public TextMeshProUGUI ButtonTexts;
    public LocalizedString textToPut;

    private void OnEnable() {
        ButtonTexts.text = textToPut.GetLocalizedString();
    }
}
