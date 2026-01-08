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
    public LayerMask Hittable;
    public LayerMask Player;
    Enemy enemy;
    Animator anim;
    public float extraRotationSpeed;
    private int i = 0;
    public float searchTime = 5f;
    private float searchTimer;
    private Vector3 lastKnownPlayerPos;

    float decisionCooldown;
    float decisionTimer;

    Combat currentAction;

    enum AIState
    {
        Patrol,
        Chase,
        Search,
        CombatState
    }

    enum Combat
    {
        Shoot,
        Advance,
        Strafe,
        TakeCover,
        Reload,
        TrowGranade,
    }
    private AIState state = AIState.Patrol;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(destinations[0].transform.position);
        agent.updateRotation = true;
        view = GetComponent<EnemyFOV>();
        anim = GetComponentInChildren<Animator>();
        enemy = GetComponent<Enemy>();

        GoToNextPatrolPoint();
        InvokeRepeating("TakeDesition",5,5);
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
            case AIState.CombatState:
                Attack();
                break;
        }
        decisionTimer -= Time.deltaTime;
        if (decisionTimer <= 0f)
        {
            DecideCombat();
            decisionTimer = decisionCooldown;
        }
    }

    public void Path()
    {
        if (view.playerSeen)
        {
            state = AIState.CombatState;
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
        CancelInvoke("TakeDesition");
        searchTimer -= Time.deltaTime;
        if (view.playerSeen)
        {
            InvokeRepeating("TakeDesition",5,5);
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

    private void DecideCombat()
    {
        float dist = Vector3.Distance(transform.position, enemy.player.transform.position);

        if (dist < 6f)
            currentAction = Combat.Strafe;
        else if (dist < 12f)
            currentAction = UnityEngine.Random.value > 0.6f
                ? Combat.Shoot
                : Combat.TakeCover;
        else
            currentAction = Combat.Advance;
    }

    private void Attack()
    {
        if (view.playerSeen)
        {
            agent.SetDestination(transform.position);
            LayerMask combinedMask = Player | Hittable ;
            lastKnownPlayerPos = enemy.player.transform.position;
            Vector3 shootDir = (lastKnownPlayerPos - transform.position).normalized;
            float distToTarget = Vector3.Distance(transform.position, lastKnownPlayerPos);

            RaycastHit Hit;
            if(Physics.Raycast(transform.position, shootDir, out Hit, distToTarget, combinedMask))
            {
                if (((1 << Hit.collider.gameObject.layer) & Player) != 0)
                {
                    Debug.Log("OUCH");
                }
                else
                {
                    Debug.Log("Failed");
                }
            }
        }
        else
        {
            searchTimer = searchTime;
            state = AIState.Search;
        }
    }
}
