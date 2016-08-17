using UnityEngine;
using GoingDark.Core.Enums;

public class Orbs : MonoBehaviour
{
    public int orbHp;
    [SerializeField]
    private BossStats boss;
    private ObjectPoolManager manager;

    void OnEnable()
    {
        if (manager == null)
            manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ObjectPoolManager>();        

        orbHp = 100;
    }

    public void MissileHit(MissileProjectile missile)
    {
        switch (missile.Type)
        {
            case MissileType.Basic:
                orbHp -= 35;
                break;
            case MissileType.Emp:
                orbHp -= 5;
                break;
            case MissileType.ShieldBreak:
                orbHp -= 5;
                break;
            case MissileType.Chromatic:
                orbHp -= 100;
                break;
        }
        missile.Kill();
        if (orbHp <= 0f)
            Kill();
    }

    public void LaserDmg(LaserProjectile laser)
    {
        switch (laser.Type)
        {
            case LaserType.Basic:
                orbHp -= 25;
                break;
            case LaserType.Charged:
                orbHp -= 50;
                break;
        }
        laser.Kill();
        if(orbHp <= 0f)
            Kill();
    }

    void Kill()
    {
        GameObject go = manager.GetEnemyExplosion();
        go.transform.position = transform.position;
        go.SetActive(true);

        boss.DecreaseOrbCount();
        gameObject.SetActive(false);
    }
}