using UnityEngine;

public class BossStats : MonoBehaviour
{
    public int BossHp;
    public int numOrbsActive;
    public bool ShieldActive;


    [SerializeField]
    private Transform Shield;
    [SerializeField]
    private Transform[] Orbs;

    private ShieldProperties shieldData;

    // Use this for initialization
    void Start()
    {
        BossHp = 100;
        numOrbsActive = Orbs.Length;
        ShieldActive = true;
        shieldData = GetComponent<EnemyStateManager>().GetShieldData();
    }

    public int GetHp()
    {
        return BossHp;
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
            Invoke("ShieldRegen", 10f);
        }
    }
    void ShieldRegen()
    {
        numOrbsActive = Orbs.Length;

        for (int x = 0; x < numOrbsActive; x++)
        {
            Orbs[x].gameObject.SetActive(true);
        }

        SetShield(true);
    }
    void Kill()
    {
        for (int x = 0; x < numOrbsActive; x++)
        {
            Orbs[x].gameObject.SetActive(false);
        }
    }
}
