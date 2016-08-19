using System;
using UnityEngine;
using GoingDark.Core.Enums;


[Serializable]
public class DebuffProperties : MonoBehaviour
{
    #region Properties
    public Impairments debuff { get; set; }

    [SerializeField]
    private GameObject stunParticles;
    [SerializeField]
    private GameObject slowParticles;
    [SerializeField]
    private PlayerMovement move;
    #endregion


    public DebuffProperties()
    {
        debuff = Impairments.None;
    }   

    public void Stun(float duration)
    {
        debuff = Impairments.Stunned;
        stunParticles.SetActive(true);
        Invoke("RemoveStun", duration);
    }

    public void Slow(float duration)
    {
        debuff = Impairments.Slowed;
        move.GetMoveData().SetBoost(.5f);
        slowParticles.SetActive(true);
        Invoke("RemoveSlow", duration);
    }

    private void RemoveSlow()
    {
        debuff = Impairments.None;
        move.GetMoveData().SetBoost(1f);
        slowParticles.SetActive(false);
    }

    private void RemoveStun()
    {
        debuff = Impairments.None;
        stunParticles.SetActive(false);
    }
}