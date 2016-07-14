using System;
using UnityEngine;

[Serializable]
public class HealthProperties
{

    [SerializeField]
    public float Health { get; private set; }

    private Transform enemyRef;

    public HealthProperties()
    {
        
    }

    public void Set(float _hp, Transform _ref)
    {
        Health = _hp;
        enemyRef = _ref;
    }

    public void Damage(float _dmg)
    {
        Health -= _dmg;
        if (Health <= 0)
            enemyRef.SendMessage("Kill");
    }
}
