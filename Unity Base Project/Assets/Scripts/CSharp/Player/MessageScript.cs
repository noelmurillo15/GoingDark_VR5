using UnityEngine;
using GoingDark.Core.Enums;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MessageScript : MonoBehaviour
{

    [SerializeField]
    Text reorientMsg;
    [SerializeField]
    Text autopilotMsg;
    [SerializeField]
    Text enemyClose;
    [SerializeField]
    Text missileInc;
    [SerializeField]
    Text poisonMsg;
    [SerializeField]
    Text offlineDevices;
    [SerializeField]
    Text systemReport;
    [SerializeField]
    Text stationTakeover;

    private Text[] winTexts;


    // Use this for initialization
    void Start()
    {
        NoWarning();
    }

    #region Msg Functions
    void NoWarning()
    {
        reorientMsg.enabled = false;
        autopilotMsg.enabled = false;
        enemyClose.enabled = false;
        missileInc.enabled = false;
        poisonMsg.enabled = false;
        systemReport.enabled = false;
        offlineDevices.enabled = false;
        stationTakeover.enabled = false;
    }
    public void SystemReport(string systemsdown)
    {
        systemReport.enabled = true;
        offlineDevices.enabled = true;
        offlineDevices.text = systemsdown;
        if (!IsInvoking("EndReport"))
            Invoke("EndReport", 10f);
    }
    public void EndReport()
    {
        systemReport.enabled = false;
        offlineDevices.enabled = false;
    }
    public void EnemyClose()
    {
        enemyClose.enabled = true;
        if (!IsInvoking("EnemyAway"))
            Invoke("EnemyAway", 5f);
    }
    void EnemyAway()
    {
        enemyClose.enabled = false;
    }
    void MissileIncoming()
    {
        missileInc.enabled = true;
    }  
    void MissileDestroyed()
    {
        missileInc.enabled = false;
    }
    public void AutoPilot()
    {
        autopilotMsg.enabled = true;
    }
    public void ManualPilot()
    {
        autopilotMsg.enabled = false;
    }
    void ReOrient()
    {
        reorientMsg.enabled = true;
        if (!IsInvoking("NoOrient"))
            Invoke("NoOrient", 4.5f);
    }
    void NoOrient()
    {
        reorientMsg.enabled = false;
    }

    void Poison()
    {
        AudioManager.instance.PlayNebulaAlarm();
        poisonMsg.enabled = true;
    }
    void NoPoison()
    {
        AudioManager.instance.StopNebulaAlarm();
        poisonMsg.enabled = false;
    }
    void StationTakeOver()
    {
        stationTakeover.enabled = true;
        Invoke("RemoveStationTakeOver", 5f);
    }
    void RemoveStationTakeOver()
    {
        stationTakeover.enabled = false;
    }
    #endregion
}