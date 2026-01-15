using UnityEngine;

public class GetCover : MonoBehaviour
{
    public Transform[] Covers;
    public bool ocupao;
    float bestDisToCover;
    Vector3 best;
    public Vector3 GetBestCover(Vector3 dirToPlayer, LayerMask Player)
    {
        bestDisToCover = Mathf.Infinity;
        foreach(Transform Cover in Covers)
        {
            float disToCover = Vector3.Distance(transform.position, Cover.position);
            Debug.DrawRay(Cover.transform.position, dirToPlayer);
            if(disToCover > bestDisToCover) continue;
            
            if(Physics.Raycast(Cover.transform.position, -dirToPlayer, out RaycastHit hit))
            {

                if (((1 << hit.collider.gameObject.layer) & Player) == 0)
                {
                    bestDisToCover = disToCover;
                    best = Cover.position;
                }                
            }
        }
        return best;
    }
}
