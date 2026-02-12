using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using Unity.VisualScripting;

public class ChangeScene : MonoBehaviour
{
    public Animator anim;
    public string sceneName;
    private AsyncOperation loadOperation;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(ElevatorRoutine());
        }
    }

    public IEnumerator ElevatorRoutine()
    {
        anim.SetTrigger("Close");
        anim.SetTrigger("Up");

        loadOperation = SceneManager.LoadSceneAsync(sceneName);
        loadOperation.allowSceneActivation = false;

        while (loadOperation.progress < 0.9f)
        {
            yield return null;
        }

        loadOperation.allowSceneActivation = true;
        yield return null;

        anim.SetTrigger("Open");
    }
}
