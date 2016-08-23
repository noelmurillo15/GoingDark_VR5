using UnityEngine;
using MovementEffects;
using System.Collections.Generic;

public class BossStats : MonoBehaviour
{
    private IEnemy stats;

    public int numOrbsActive;
    private int maxOrbs = 4;

    [SerializeField]
    private GameObject[] Orbs;
    [SerializeField]
    private GameObject[] burns;


    void Start()
    {
        stats = GetComponent<IEnemy>();
        burns[0].SetActive(false);
        burns[1].SetActive(false);
        burns[2].SetActive(false);

        numOrbsActive = maxOrbs;

        Timing.RunCoroutine(CheckHealth());
    }

    public void DecreaseOrbCount()
    {
        numOrbsActive--;
        if (numOrbsActive <= 0)
        {
            stats.GetShieldData().SetShieldActive(false);
            numOrbsActive = maxOrbs;        
            Invoke("ShieldRegen", 10f);
        }
    }
    void ShieldRegen()
    {
        for (int x = 0; x < numOrbsActive; x++)
            Orbs[x].SetActive(true);

        stats.GetShieldData().SetShieldActive(true);
    }

    private IEnumerator<float> CheckHealth()
    {
        while (true)
        {
            if (stats.GetHealthData() != null)
            {
                float hp = stats.GetHealthData().Health / stats.GetHealthData().MaxHealth;

                if (hp <= .75f)
                    burns[0].SetActive(true);
                else if (hp <= .5f)
                    burns[1].SetActive(true);
                else if (hp <= .25f)
                    burns[2].SetActive(true);
            }

            yield return Timing.WaitForSeconds(1f);
        }
    }
}