using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    #region Properties
    public Transform Target { get; set; }
    #endregion

    public void FoundTarget(Transform _target, Vector3 enemypos)
    {
        Target = _target;
        object[] tempStorage = new object[2];
        tempStorage[0] = Target;
        tempStorage[1] = enemypos;
        BroadcastMessage("BroadcastAlert", tempStorage);
    }

    public void TargetDestroyed()
    {
        BroadcastMessage("BroadcastWin");
    }
}