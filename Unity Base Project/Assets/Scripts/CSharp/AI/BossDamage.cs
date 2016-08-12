using UnityEngine;
using MovementEffects;
using System.Collections.Generic;

public class BossDamage : MonoBehaviour
{
    [SerializeField]
    private IEnemy st;

    [SerializeField]
    private GameObject []burns;
    
    void Start()
    {
        burns[0].SetActive(false);
        burns[1].SetActive(false);
        burns[2].SetActive(false);

        Timing.RunCoroutine(CheckHealth());
    }

    private IEnumerator<float> CheckHealth()
    {
        while (true)
        {
            if (st.GetHealthData() != null)
            {
                float hp = st.GetHealthData().Health / st.GetHealthData().MaxHealth;

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