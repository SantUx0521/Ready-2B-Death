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

    [SerializeField] float decisionCooldown;
    [SerializeField] float decisionTimer;

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
    }

    void Update()
    {
        switch (state)
        {
            case AIState.Patrol:
                Path();
                break;

            case AIState.Search:
                Search();
                break;
            case AIState.CombatState:
                CombatMode();
                break;
        }
    }

    public void CombatMode()
    {
        decisionTimer -= Time.deltaTime;
        if (decisionTimer <= 0f)
        {
            DecideCombat();
            decisionTimer = decisionCooldown;
        }

        switch (currentAction)
        {
            case Combat.Shoot:
                Shoot();
                break;
            case Combat.Advance:
                Chase();
                break;
            case Combat.Strafe:
                Strafe();
                break;
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
        agent.SetDestination(enemy.player.transform.position);
    }

    private void Search()
    {
        searchTimer -= Time.deltaTime;
        if (view.playerSeen)
        {
            
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

        if (dist < 40f)
            currentAction = UnityEngine.Random.value > 0.6f
                ? Combat.Shoot
                : Combat.Strafe;
        else
            currentAction = Combat.Advance;
    }

    private void Shoot()
    {
        LookAtPlayer();
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
                    Debug.Log("te voy a pegar");
                    StartCoroutine(BeDummier());
                }
            }
        }
        else
        {
            return;
        }
    }

    private IEnumerator BeDummier()
    {
        Debug.Log("decido");
        int madeIt = UnityEngine.Random.Range(0,5);
        if (madeIt > 2)
        {
            Debug.Log("Te jodiste");
        }
        else
        {
            Debug.Log("Fallo el tiro");
        }
        yield return new WaitForSeconds(1);
    }

    private void Strafe()
    {
        Vector3 dirToPlayer = (transform.position - enemy.player.transform.position).normalized;
        Vector3 strafeDir = Vector3.Cross(Vector3.up, dirToPlayer);

        if(UnityEngine.Random.value > 0.5f)
        {
            strafeDir = -strafeDir;
        } 
        agent.SetDestination(transform.position + strafeDir * 4f);
    }

    void LookAtPlayer()
    {
        Vector3 dir = enemy.player.transform.position - transform.position;
        dir.y = 0f; // solo rotar en Y

        if (dir.sqrMagnitude < 0.001f) return;

        Quaternion targetRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            Time.deltaTime * 8f
        );
    }

}
