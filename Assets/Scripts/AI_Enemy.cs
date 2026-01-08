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

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(destinations[0].transform.position);
        view = GetComponent<EnemyFOV>();
        anim = GetComponentInChildren<Animator>();
        enemy = GetComponent<Enemy>();
    }

    void Update()
    {
        EnemyShoot();
        extraRotation();
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

    private void EnemyShoot()
    {
        if (view.playerSeen)
        {
            agent.SetDestination(enemy.player.transform.position);
            StartCoroutine(BackToPath());
        }
        else
        {
            Path();
        }
    }

    private IEnumerator BackToPath()
    {
        yield return new WaitForSeconds(10f);
    }

    void extraRotation()
    	{
    		Vector3 lookrotation = agent.steeringTarget-transform.position;
    		transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(lookrotation), extraRotationSpeed*Time.deltaTime);
    
    	}
}
