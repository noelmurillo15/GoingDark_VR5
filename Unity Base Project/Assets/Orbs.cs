using UnityEngine;
using System.Collections;

public class Orbs : MonoBehaviour
{
    [SerializeField]
    private BossStats boss;

    private Transform myTransform;
    private int orbHp;
    // Use this for initialization
    void Start()
    {
        orbHp = 100;
        myTransform = transform;
    }

    public void DMG()
    {
        Debug.Log("Hit Orb");
        orbHp -= 100;

        if(orbHp <= 0f)
            Kill();
    }

    void Kill()
    {
        Debug.Log("Orb Destroyed");
        boss.SendMessage("DecreaseOrbCount");
        gameObject.SetActive(false);
    }
}