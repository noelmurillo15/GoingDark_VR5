using UnityEngine;

[RequireComponent(typeof(EnemyBehavior))]
public class TetherAi : MonoBehaviour
{
    #region Properties
    //  Enemy Data
    private EnemyBehavior behavior;
    #endregion


    // Use this for initialization
    void Start()
    {
        behavior = GetComponent<EnemyBehavior>();
        behavior.SetUniqueAi(this);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}