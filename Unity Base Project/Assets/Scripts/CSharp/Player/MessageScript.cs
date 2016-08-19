using UnityEngine;
using UnityEngine.UI;
using GoingDark.Core.Enums;

public class MessageScript : MonoBehaviour
{

    [SerializeField]
    Text enemyClose;
    [SerializeField]
    Text missileInc;
    [SerializeField]
    Text stunMsg;
    [SerializeField]
    Text slowMsg;
    [SerializeField]
    Text offlineDevices;
    [SerializeField]
    Text systemReport;
    [SerializeField]
    Text stationTakeover;
    [SerializeField]
    Text systemInitialized;

    private Text[] winTexts;


    // Use this for initialization
    void Awake()
    {
        NoWarning();
    }

    #region Msg Functions
    void NoWarning()
    {
        slowMsg.enabled = false;
        enemyClose.enabled = false;
        missileInc.enabled = false;
        stunMsg.enabled = false;
        systemReport.enabled = false;
        offlineDevices.enabled = false;
        stationTakeover.enabled = false;
        systemInitialized.enabled = false;
    }
    public void SystemReport(SystemType systemsdown)
    {
        systemReport.enabled = true;
        offlineDevices.enabled = true;
        offlineDevices.text = systemsdown.ToString();
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
        if (IsInvoking("EnemyAway"))
            CancelInvoke("EnemyAway");

        enemyClose.enabled = true;
        Invoke("EnemyAway", 10f);
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

        Invoke("MissileDestroyed", 1f);
    }  
    public void MissileDestroyed()
    {
        missileInc.enabled = false;
    }
    public void Stun(float _duration)
    {
        stunMsg.enabled = true;
        Invoke("NotStunned", _duration);
    }    
    public void Slow(float _duration)
    {
        slowMsg.enabled = true;
        Invoke("NotSlowed", _duration);
    }
    public void NotStunned()
    {
        stunMsg.enabled = false;
    }
    public void NotSlowed()
    {
        slowMsg.enabled = false;
    }
    public void Init(string system)
    {
        systemInitialized.text = "System Initialized: " + system;
        systemInitialized.enabled = true;
    }
    public void InitDown()
    {
        systemInitialized.enabled = false;
    }
    void StationTakeOver()
    {
        stationTakeover.enabled = true;
        Invoke("RemoveStationTakeOver", 10f);
    }
    void RemoveStationTakeOver()
    {
        stationTakeover.enabled = false;
    }
    #endregion
}