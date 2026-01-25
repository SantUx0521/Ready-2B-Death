using UnityEngine;

public class GetGlock : MonoBehaviour
{
    public string wepname = "Glock";
    GetWeapon getWeapon;

    void Start()
    {
        getWeapon.GetComponent<GetWeapon>();
        getWeapon.wepname = wepname;
    }
}
