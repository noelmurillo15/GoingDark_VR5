using UnityEngine;

public class TetherAi : MonoBehaviour
{
    #region Properties
    //  Enemy Data
    private EnemyStateManager behavior;
    #endregion


    // Use this for initialization
    void Start()
    {
        behavior = GetComponent<EnemyStateManager>();
        behavior.SetUniqueAi(this);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}