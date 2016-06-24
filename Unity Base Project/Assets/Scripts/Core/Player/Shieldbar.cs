using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Shieldbar : MonoBehaviour {

    //    [SerializeField]
    public Image ShieldCircle;
    [SerializeField]
    float CurrentShield = 0.0f;
    [SerializeField]
    float Max_Shield_Health = 100.0f;



    void Start()
    {
        CurrentShield = Max_Shield_Health;
    }

    /// Damage is based off 100% so taking 20 damage is 1/5 of full health/sheild
    /// try to stick with 20 health ticks, means 5 hits before death.
    public void DecreaseShield(float DamageTaken)
    {
        CurrentShield -= DamageTaken;
        float C_Shield = CurrentShield / Max_Shield_Health;
        C_Shield *= .5f;
        SetHealth(C_Shield);
    }


    void SetHealth(float NewShield)
    {
        ShieldCircle.fillAmount = NewShield;
    }


    public void Reset()
    {
      //  Debug.Log("ShieldBar : Reset()");
        CurrentShield = Max_Shield_Health;

        SetHealth(((CurrentShield / Max_Shield_Health) * 0.5f));
    }
}
