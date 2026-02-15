using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    public void Continuar()
    {
        StartCoroutine(PlayAnimationThenReload());
    }

    public void BackToMenu()
    {
        StartCoroutine(PlayAnimationThenLoad("Main Menu"));
    }

    private IEnumerator PlayAnimationThenLoad(string sceneName)
    {
        yield return new WaitForSecondsRealtime(1.5f);
        Time.timeScale = 1;
        gameObject.SetActive(false);
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator PlayAnimationThenReload()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        Time.timeScale = 1;
        gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
