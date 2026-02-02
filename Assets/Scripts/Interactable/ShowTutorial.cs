using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class ShowTutorial : MonoBehaviour
{
    public GameObject tutorialUI;
    public TextMeshProUGUI textUI;
    public LocalizedString textToPut;

    public Image ImageUI;
    public Sprite button;
    private Animator anim;

    void Start()
    {
        anim = tutorialUI.GetComponent<Animator>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            textUI.text = textToPut.GetLocalizedString();
            ImageUI.sprite = button;
            tutorialUI.SetActive(true);
            textUI.gameObject.SetActive(true);
            ImageUI.gameObject.SetActive(true);
        }
    }
        

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(StopIt());
        }
    }

    private IEnumerator StopIt()
    {
        anim.SetTrigger("Hide");
        yield return new WaitForSeconds(1f);
        tutorialUI.SetActive(false);
        textUI.gameObject.SetActive(false);
        ImageUI.gameObject.SetActive(false);
    }
}
