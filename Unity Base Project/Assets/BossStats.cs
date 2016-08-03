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

    // Use this for initialization
    void Start()
    {
        BossHp = 100;
        numOrbsActive = 3;
        ShieldActive = true;
    }

    public int GetHp()
    {
        return BossHp;
    }
    public void SetShield(bool flip)
    {
        ShieldActive = flip;
        Shield.gameObject.SetActive(ShieldActive);
    }

    void DecreaseOrbCount()
    {
        Debug.Log("Orb count decreased");
        numOrbsActive--;
        if (numOrbsActive <= 0)
        {
            Debug.Log("Boss SHield down");
            SetShield(false);
            Invoke("ShieldRegen", 10f);
        }
    }
    void ShieldRegen()
    {
        Debug.Log("Boss SHield regen");

        for (int x = 0; x < 3; x++)
            Orbs[x].gameObject.SetActive(true);

        numOrbsActive = 3;
        SetShield(true);
    }
    void Kill()
    {
        Destroy(gameObject);
    }
}
