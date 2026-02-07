using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class AI_Enemy : MonoBehaviour
{
    [Header("AI")]
    private NavMeshAgent agent;
    public Transform[] destinations;
    public GameObject destinationsParent;
    private EnemyFOV view;
    public LayerMask Hittable;
    public LayerMask Player;
    Enemy enemy;
    Animator anim;
    private int i = 0;
    public float searchTime = 5f;
    private float searchTimer;
    private Vector3 lastKnownPlayerPos;
    public GameObject fire;
    public GameObject Firelight;
    [SerializeField] float decisionCooldown;
    [SerializeField] float decisionTimer;
    private bool beDummierIsRunning = false;
    public float waiting;
    Combat currentAction;
    [Header("Audio")]
    [SerializeField] AudioMixer shootAudioMixer;
    [SerializeField] AudioClip shootSound;
    public AudioSource audioSource;

    [Header("Cover")]
    bool movingToCover;
    GetCover getCover;
    private bool stay;
    float distToTarget;
    public bool haveGranades;
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
        Stalk,
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
        anim = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();
        getCover = GetComponent<GetCover>();

        GoToNextPatrolPoint();
    }

    void OnValidate()
    {
        if(destinationsParent != null)
        {
            int childs = destinationsParent.transform.childCount;
            destinations = new Transform[childs];
            for (int i = 0; i < childs; i++)
            { 
                destinations[i] = destinationsParent.transform.GetChild(i);
            }
        }
    }

    void Update()
    {
        if (enemy.isDead) 
        {
            agent.isStopped = true;
            return;
        }
        
        switch (state)
        {
            case AIState.Patrol:
                Walking();
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
                stay = true;
                Shoot();
                break;
            case Combat.Advance:
                Chase();
                break;
            case Combat.TakeCover:
                stay = false;
                TakeCover();
                if (movingToCover && agent.remainingDistance < 0.5f)
                {
                    movingToCover = false;
                }
                break;
            case Combat.TrowGranade:
                stay = true;
                LauchGranade();
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
        Walking();
    }

    private void Search()
    {
        Walking();
        searchTimer -= Time.deltaTime;
        if (view.playerSeen)
        {
            state = AIState.CombatState;
        }
        else if(searchTimer <= 0)
        {
            state = AIState.Patrol;
            GoToNextPatrolPoint();
        }
    }

    private void GoToNextPatrolPoint()
    {
        if(destinations[i] == null)
        {
            Debug.Log("Nada");
        }
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
        float randint = Random.value;
        if (dist < 21f)
            if( randint > 0.6f)
            {
                currentAction = Combat.Shoot;
            }
            else if (randint < 0.6 && randint > 0.1)
            {
                currentAction = Combat.TakeCover;
            }
            else
            {
                currentAction = Combat.TrowGranade;
            }
                
        else
            currentAction = Combat.Advance;
    }

    private void Shoot()
    {
        LookAtPlayer();
        if (view.playerSeen)
        {
            if (stay)
            {
                Walking();
                agent.SetDestination(transform.position);
            }
            LayerMask combinedMask = Player | Hittable ;
            lastKnownPlayerPos = enemy.player.transform.position;
            Vector3 shootDir = (lastKnownPlayerPos - transform.position).normalized;
            distToTarget = Vector3.Distance(transform.position, lastKnownPlayerPos);

            RaycastHit Hit;
            if(Physics.Raycast(transform.position, shootDir, out Hit, distToTarget, combinedMask))
            {
                if (((1 << Hit.collider.gameObject.layer) & Player) != 0)
                {
                    StartCoroutine(BeDummier(Hit));
                }
            }
        }
        else
        {
            return;
        }
    }
    private IEnumerator BeDummier(RaycastHit Hit) //Quiero hacer más tonta a la IA porque si le dejo pegarlos todos se vuelve exagerada la dificulta XD
    {
        if (beDummierIsRunning) yield break;
        beDummierIsRunning = true;

        yield return new WaitForSeconds(waiting);

        fire.SetActive(true);
        Firelight.SetActive(true);
        audioSource.Stop();
        audioSource.PlayOneShot(shootSound);
        Vector3 currentEuler = fire.transform.localEulerAngles;
        currentEuler.y = Random.Range(-180, 180);
        fire.transform.localEulerAngles = currentEuler;
        yield return new WaitForSeconds(0.1f);
        float dynamicAim = 5;
        if (distToTarget < 5)
        {
            dynamicAim = 8;
        }
        else if (distToTarget > 10)
        {
            dynamicAim = 2;
        }

        float madeIt = Random.Range(0, 10);
        if (madeIt < dynamicAim)
        {
            Hit.collider.gameObject.GetComponent<GameManager>().TakeDamage(enemy.damage); //Listo mai brodel
        }
        else
        {
             // Ahora queda pendiente esto porque tecnicamente quiero que haga chispas o algo de impacto contra superficie cerca del player i guess
        }
        fire.SetActive(false);
        Firelight.SetActive(false);
        beDummierIsRunning = false;
    }

    private void TakeCover()
    {
        LookAtPlayer();
        Walking();
        Shoot();
        Vector3 playerPos = enemy.player.transform.position;
        float disToPlayer = Vector3.Distance(transform.position, enemy.player.transform.position);
        Vector3 bestCover = getCover.GetBestCover(playerPos, Player);

        if (!movingToCover && disToPlayer > 10) //Queda medio mal pero solo con un else no garantizo que dispare cuando tiene que hacerlo
        {
            agent.SetDestination(bestCover);
            movingToCover = true;
        }
        else if (!movingToCover && disToPlayer < 10)
        {
            currentAction = Combat.Shoot;
        }
    }

    private void LauchGranade()
    {
        if (haveGranades)
        {
            Vector3 dir = enemy.player.transform.position - transform.position; 
            GameObject newGranade = Instantiate(enemy.granade, transform.position, transform.rotation);
            newGranade.GetComponent<Rigidbody>().AddForce(dir * 700);
            currentAction = Combat.Shoot;
        }
        else
        {
            currentAction = Combat.Shoot;
        }
    }

    void LookAtPlayer()
    {
        Vector3 dir = enemy.player.transform.position - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.001f) return;

        Quaternion targetRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            Time.deltaTime * 8f
        );
    }

    private void Hear(Vector3 noisePos, float maxRadious)
    {
        float distToNoise = Vector3.Distance(transform.position, noisePos);
        if(maxRadious < distToNoise) return;

        lastKnownPlayerPos = noisePos;
        state = AIState.CombatState;
        DecideCombat();

        if(!view.playerSeen)
        {
            agent.SetDestination(lastKnownPlayerPos);
        }  
    }
    void OnEnable()
    {
        WeaponController.OnNoise += OnNoiseHeard;
    }

    void OnDisable()
    {
        WeaponController.OnNoise -= OnNoiseHeard;
    }

    private void OnNoiseHeard(Vector3 noisePos, float radius)
    {
        if(enemy.isDead) return;
        Hear(noisePos, radius);
    }

    private void Walking()
    {
        bool notMoving = agent.velocity.sqrMagnitude < 0.01f || (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance);
        
        if (notMoving)
        {
            anim.SetBool("Walking", false);
            anim.SetBool("WalkingBack", false);
            return;
        }

        Vector3 moveDirection = agent.velocity.normalized;
        float dot = Vector3.Dot(transform.forward, moveDirection);
        
        if (dot > 0)
        {
            anim.SetBool("Walking", true);
            anim.SetBool("WalkingBack", false);
        }
        else
        {
            anim.SetBool("Walking", false);
            anim.SetBool("WalkingBack", true);
        }
    }
}
