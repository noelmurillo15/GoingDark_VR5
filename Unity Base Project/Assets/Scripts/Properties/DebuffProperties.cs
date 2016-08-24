using System;
using UnityEngine;
using GoingDark.Core.Enums;


[Serializable]
public class DebuffProperties : MonoBehaviour
{
    #region Properties
    [SerializeField]
    private GameObject stunParticles;
    [SerializeField]
    private GameObject slowParticles;
    [SerializeField]
    private PlayerMovement move;
    #endregion


    public DebuffProperties()
    {

    }   

    public void Stun(float duration)
    {
        if (IsInvoking("RemoveStun"))
            CancelInvoke("RemoveStun");

        move.GetMoveData().SetMaxSpeed(0f);
        stunParticles.SetActive(true);
        Invoke("RemoveStun", duration);
    }
    private void RemoveStun()
    {
        move.GetMoveData().SetMaxSpeed(100f);
        stunParticles.SetActive(false);
    }

    public void Slow(float duration)
    {
        if (IsInvoking("RemoveSlow"))
            CancelInvoke("RemoveSlow");

        move.GetMoveData().SetBoost(.5f);
        slowParticles.SetActive(true);
        Invoke("RemoveSlow", duration);
    }
    private void RemoveSlow()
    {
        move.GetMoveData().SetBoost(1f);
        slowParticles.SetActive(false);
    }    
}