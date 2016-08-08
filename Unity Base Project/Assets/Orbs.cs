﻿using UnityEngine;
using GoingDark.Core.Enums;

public class Orbs : MonoBehaviour
{
    public int orbHp;
    [SerializeField]
    private BossStats boss;


    void OnEnable()
    {
        orbHp = 100;
    }

    public void MissileDmg(Missile missile)
    {
        switch (missile.Type)
        {
            case MissileType.Basic:
                orbHp -= 20;
                break;
            case MissileType.Emp:
                orbHp -= 10;
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
                orbHp -= 10;
                break;
            case LaserType.Charged:
                orbHp -= 25;
                break;
            case LaserType.Ball:
                orbHp -= 50;                
                break;
            case LaserType.Enemy:
                orbHp -= 5;
                break;
            default:
                break;
        }
        if(orbHp <= 0f)
            Kill();
    }

    void Kill()
    {
        boss.DecreaseOrbCount();
        gameObject.SetActive(false);
    }
}