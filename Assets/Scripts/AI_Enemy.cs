using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

public class AI_Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform[] destinations;
    private EnemyFOV view;
    Enemy enemy;
    Animator anim;
    public float extraRotationSpeed;
    private int i = 0;
    public float searchTime = 5f;
    private float searchTimer;
    private Vector3 lastKnownPlayerPos;

    enum AIState
    {
        Patrol,
        Chase,
        Search
    }
    private AIState state = AIState.Patrol;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(destinations[0].transform.position);
        agent.updateRotation = false;
        view = GetComponent<EnemyFOV>();
        anim = GetComponentInChildren<Animator>();
        enemy = GetComponent<Enemy>();

        GoToNextPatrolPoint();
    }

    void Update()
    {
        switch (state)
        {
            case AIState.Patrol:
                Path();
                break;

            case AIState.Chase:
                Chase();
                break;

            case AIState.Search:
                Search();
                break;
        }

        extraRotation();
    }

    public void Path()
    {
        if (view.playerSeen)
        {
            state = AIState.Chase;
            return;
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            GoToNextPatrolPoint();
        }
    }

    private void Chase()
    {
        if (view.playerSeen)
        {
            agent.SetDestination(enemy.player.transform.position);
            lastKnownPlayerPos = enemy.player.transform.position;
        }
        else
        {
            searchTimer = searchTime;
            agent.SetDestination(lastKnownPlayerPos);
            state = AIState.Search;
        }
    }

    private void Search()
    {
        searchTimer -= Time.deltaTime;
        if (view.playerSeen)
        {
            state = AIState.Chase;
        }
        else if(searchTimer <= 0)
        {
            state = AIState.Patrol;
            GoToNextPatrolPoint();
        }
    }

    private void GoToNextPatrolPoint()
    {
        agent.SetDestination(destinations[i].transform.position);
        i++;
        if (i >= destinations.Length)
            {
                i = 0;
            }
    }

    void extraRotation()
    {
            if (agent.velocity.sqrMagnitude < 0.1f) return;

            Vector3 dir = agent.velocity.normalized;
            Quaternion targetRot = Quaternion.LookRotation(dir);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                extraRotationSpeed * Time.deltaTime
            );
    }
}
