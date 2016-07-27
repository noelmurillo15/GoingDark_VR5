using System;
using UnityEngine;


[Serializable]
public class DebuffProperties
{
    #region Properties
    public bool isSlowed { get; set; }
    public bool isStunned { get; set; }
    #endregion


    public DebuffProperties()
    {
        isSlowed = false;
        isStunned = false;
    }   
}