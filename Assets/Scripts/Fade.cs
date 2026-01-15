using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public void FadeOut(Image image, float duration, float iconDuration)
    {
        image.gameObject.SetActive(true);
        StartCoroutine(gradiantOut(image, duration, iconDuration));
    }

    public IEnumerator gradiantOut(Image image, float duration, float iconDuration)
    {
        Color color = image.color;
        float timer = 0f;
        color.a = 0f;
        image.color = color;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Clamp01(Mathf.Lerp(0f, 1f, timer / duration));
            image.color = color;
            yield return null;
        }
        color.a = 1f;
        image.color = color;

        yield return new WaitForSeconds(iconDuration);

        timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Clamp01(Mathf.Lerp(1f, 0f, timer / duration));
            image.color = color;
            yield return null;
        }

        color.a = 0f;
        image.color = color;
        image.gameObject.SetActive(false);
    }
}
