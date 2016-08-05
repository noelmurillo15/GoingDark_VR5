using UnityEngine;
using System.Collections;

public class Orbs : MonoBehaviour
{
    public int orbHp;
    [SerializeField]
    private BossStats boss;


    void OnEnable()
    {
        orbHp = 100;
    }

    public void DMG()
    {
        orbHp -= 25;
        if(orbHp <= 0f)
            Kill();
    }

    void Kill()
    {
        boss.DecreaseOrbCount();
        gameObject.SetActive(false);
    }
}