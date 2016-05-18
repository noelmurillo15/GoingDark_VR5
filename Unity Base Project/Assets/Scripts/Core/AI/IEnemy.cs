using UnityEngine;
using GD.Core.Enums;

public class IEnemy : MonoBehaviour
{

    #region Properties
    public EnemyTypes Type;    
    public Transform Target;
    public Transform MyTransform; 
    #endregion


    public virtual void Init()
    {
        Debug.Log("IEnemy Initialize Called");
        MyTransform = transform;
        Type = EnemyTypes.NONE;       
        Target = null;
    }   

    #region Modifiers
    public void SetEnemyType(EnemyTypes _type)
    {
        Debug.Log("Setting Enemy Type : " + _type);
        Type = _type;
    }
    public void SetEnemyTarget(Transform _target)
    {
        Debug.Log("Setting Enemy Target : " + _target.name);
        Target = _target;
    }    
    #endregion

    #region Msg Functions
    public void EMPHit()
    {
        Debug.Log("Enemy has been hit by Emp");
    }

    public void Hit()
    {
        Debug.Log("Enemy has creashed with an Asteroid");
    }

    public void Kill()
    {
        Debug.Log("Enemy has been Destroyed");
        Destroy(gameObject);
    }
    #endregion
}