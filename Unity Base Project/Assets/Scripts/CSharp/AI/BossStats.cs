﻿using UnityEngine;

public class BossStats : MonoBehaviour
{
    public int numOrbsActive;
    public bool ShieldActive;

    [SerializeField]
    private GameObject[] Orbs;

    private ShieldProperties shieldData;

    // Use this for initialization
    void Start()
    {
        ShieldActive = true;
        numOrbsActive = Orbs.Length;
        Invoke("GetShield", 1.5f);        
    }

    void GetShield()
    {
        shieldData = GetComponent<IEnemy>().GetShieldData();
    }

    public void SetShield(bool flip)
    {
        ShieldActive = flip;
        shieldData.SetShieldActive(ShieldActive);
    }

    public void DecreaseOrbCount()
    {
        numOrbsActive--;
        if (numOrbsActive <= 0)
        {
            SetShield(false);
            numOrbsActive = Orbs.Length;
            Invoke("ShieldRegen", 10f);
        }
    }

    void ShieldRegen()
    {
        SetShield(true);
        for (int x = 0; x < numOrbsActive; x++)
            Orbs[x].SetActive(true);        
    }
}