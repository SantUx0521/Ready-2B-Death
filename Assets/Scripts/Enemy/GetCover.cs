using UnityEngine;

public class GetCover : MonoBehaviour
{
    public Transform[] Covers;
    public GameObject coversParent;
    public GameObject lastCover;
    float bestDisToCover;
    Vector3 best;

    void OnValidate()
    {
        if(coversParent != null)
        {
            int childsC = coversParent.transform.childCount;
            Covers = new Transform[childsC];
            for (int i = 0; i < childsC; i++)
            { 
                Covers[i] = coversParent.transform.GetChild(i);
            }
        } 
    }
    public Vector3 GetBestCover(Vector3 playerPos, LayerMask Player)
    {
        bestDisToCover = Mathf.Infinity;
        foreach(Transform Cover in Covers)
        {
            if(!Cover.TryGetComponent<IsCoverOcupao>(out var coverPoint) || coverPoint.ocupao) 
            continue;

            float disToCover = Vector3.Distance(transform.position, Cover.position);
            if(disToCover > bestDisToCover) continue;
            
            Vector3 dirToPlayer = (Cover.position - playerPos).normalized;
            
            if(Physics.Raycast(Cover.transform.position, -dirToPlayer, out RaycastHit hit))
            {
                if (((1 << hit.collider.gameObject.layer) & Player) == 0)
                {
                    bestDisToCover = disToCover;
                    lastCover = Cover.gameObject;
                    best = Cover.position;
                }                
            }
        }
        lastCover.GetComponent<IsCoverOcupao>().ocupao = true;
        return best;
    }
}
