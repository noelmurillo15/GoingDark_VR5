using UnityEngine;

public class TetherAi : MonoBehaviour
{
    #region Properties
    LineRenderer daLine;
    Vector3[] pos = new Vector3[2];
    private EnemyStateManager behavior;
    #endregion

    void Start()
    {
        daLine = GetComponent<LineRenderer>();
        behavior = GetComponent<EnemyStateManager>();
    }

    void FixedUpdate()
    {

        if (behavior.State == GoingDark.Core.Enums.EnemyStates.Attack)
        {
            pos[0] = transform.position;
            pos[1] = behavior.Target.position;
            daLine.SetPositions(pos);
            //AudioManager.instance.PlayTether();*
        }
        else
        {
            pos[0] = transform.position;
            pos[1] = transform.position;
            daLine.SetPositions(pos);
            //AudioManager.instance.StopTether();*
        }

    }   
}
// *Put back in when better Tether audio is found, the current is very cracky