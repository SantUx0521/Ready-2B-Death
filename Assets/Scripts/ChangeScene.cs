using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    public Animator anim;
    public string sceneName;
    private bool hasLoaded = false;
    public GameObject Elevator;
    [SerializeField] private Vector3 elevPos;
    [SerializeField] private Vector3 PlayerPos;
    private AsyncOperation loadOperation;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !hasLoaded)
        {
            StartCoroutine(ElevatorRoutine(other.gameObject));
        }
    }

    public IEnumerator ElevatorRoutine(GameObject player)
    {
        anim.SetTrigger("Close");
        anim.SetTrigger("Up");
        yield return new WaitForSecondsRealtime(5f);

        loadOperation = SceneManager.LoadSceneAsync(sceneName);
        loadOperation.allowSceneActivation = false;

        while (loadOperation.progress < 0.9f)
        {
            yield return null;
        }

        loadOperation.allowSceneActivation = true;
        Elevator.transform.position = elevPos;
        player.transform.position = PlayerPos;
        yield return new WaitForSecondsRealtime(3f);

        anim.SetTrigger("Open");
        hasLoaded = true;
    }
}
