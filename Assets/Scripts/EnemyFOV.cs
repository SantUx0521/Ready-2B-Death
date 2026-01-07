using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Behavior;
public class EnemyFOV : MonoBehaviour
{
    public float viewRadius;
	[Range(0,360)]
	public float viewAngle;
    public bool playerSeen;
	public BehaviorGraphAgent agent;
	public LayerMask target;
	public LayerMask obstacle;
    [HideInInspector] public List<Transform> visibleTargets = new List<Transform>();

    public Vector3 DirFromAngle(float angle, bool angleIsGlobal)
    {
        if (!angleIsGlobal) {
			angle += transform.eulerAngles.y;
		}
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
    void Start()
    {
        StartCoroutine ("FindTargetsWithDelay", .2f);
		agent = GetComponent<BehaviorGraphAgent>();
    }
    void FindVisibleTargets() {
		visibleTargets.Clear();
        playerSeen = false;
		agent.BlackboardReference.SetVariableValue("playerSeen", false);
		Collider[] targetsInViewRadius = Physics.OverlapSphere (transform.position, viewRadius, target);

		for (int i = 0; i < targetsInViewRadius.Length; i++) {
			Transform target = targetsInViewRadius [i].transform;
			Vector3 dirToTarget = (target.position - transform.position).normalized;
			if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2) {
				float dstToTarget = Vector3.Distance(transform.position, target.position);

				if (!Physics.Raycast (transform.position, dirToTarget, dstToTarget, obstacle)) {
					visibleTargets.Add(target);
				}
			}
		}
        foreach (Transform visibleTarget in visibleTargets) {
			playerSeen = true;
			agent.BlackboardReference.SetVariableValue("playerSeen", true);
		}
	}
    IEnumerator FindTargetsWithDelay(float delay) {
		while (true) {
			yield return new WaitForSeconds(delay);
			FindVisibleTargets();
		}
	}
}
