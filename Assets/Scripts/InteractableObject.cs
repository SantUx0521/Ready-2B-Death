using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    public MeshRenderer render;
    public Material OutlineMaterial;
    public Material NormalMaterial;
    PlayerController isPlayerLooking;
    public bool state = false;

    void Start()
    {
        render = GetComponent<MeshRenderer>();    
        isPlayerLooking = FindAnyObjectByType<PlayerController>(); 
    }
    void Update()
    {
        OutLine();
        state = false;
    }

    public void OutLine()
    {
        Material[] mats = render.materials;
        if (state)
        {
            mats[1] = OutlineMaterial;
        }
        else
        {
            mats[1] = NormalMaterial;
        }
        render.materials = mats;
    }
}
