using UnityEngine;
using UnityEngine.UI;

public class MessageScript : MonoBehaviour
{

    [SerializeField]
    Text enemyClose;
    [SerializeField]
    Text missileInc;
    [SerializeField]
    Text stunMsg;
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
        enemyClose.enabled = false;
        missileInc.enabled = false;
        stunMsg.enabled = false;
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
    public void MissileIncoming()
    {
        missileInc.enabled = true;
        if(IsInvoking("MissileDestroyed"))
            CancelInvoke("MissileDestroyed");

        Invoke("MissileDestroyed", 5f);
    }  
    public void MissileDestroyed()
    {
        missileInc.enabled = false;
    }
    public void Stun()
    {
        stunMsg.enabled = true;
    }
    public void NotStunned()
    {
        stunMsg.enabled = false;
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