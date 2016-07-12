using System;
using UnityEngine;

[Serializable]
public class HealthProperties
{

    [SerializeField]
    public int Health { get; private set; }

    private Transform enemyRef;

    public HealthProperties()
    {
        
    }

    public void Set(int _hp, Transform _ref)
    {
        Health = _hp;
        enemyRef = _ref;
    }

    public void Damage(int _dmg)
    {
        Health -= _dmg;
        if (Health <= 0)
            enemyRef.SendMessage("Kill");
    }
}
