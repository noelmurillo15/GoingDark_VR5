using UnityEngine;

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
        shieldData = GetComponent<EnemyStateManager>().GetShieldData();
    }

    public void SetShield(bool flip)
    {
        Debug.Log("Titan Shield Turning : " + flip);
        ShieldActive = flip;
        shieldData.SetShieldActive(ShieldActive);
    }

    public void DecreaseOrbCount()
    {
        Debug.Log("Decreasing Orb Count");
        numOrbsActive--;
        if (numOrbsActive <= 0)
        {
            Invoke("ShieldRegen", 10f);
            SetShield(false);
        }
    }

    void ShieldRegen()
    {
        Debug.Log("Titan Shield Regen");
        SetShield(true);
        numOrbsActive = Orbs.Length;
        for (int x = 0; x < numOrbsActive; x++)
            Orbs[x].SetActive(true);        
    }
}
