using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

public class AI_Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform[] destinations;
    private int i = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(destinations[0].transform.position);
    }

    void Update()
    {
        Path();
    }

    public void Path()
    {
        agent.SetDestination(destinations[i].transform.position);
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            i++;
            if (i >= destinations.Length)
            {
                i = 0;
            }
        }
    }
}
