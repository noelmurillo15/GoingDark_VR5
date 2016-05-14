using UnityEngine;
using GD.Core.Enums;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MessageScript : MonoBehaviour
{
    [SerializeField]
    GameObject winMessage;

    [SerializeField]
    Text reorientMsg;
    [SerializeField]
    Text autopilotMsg;
    [SerializeField]
    Text enemyClose;
    [SerializeField]
    Text missileInc;
    [SerializeField]
    Text collectedLoot;
    [SerializeField]
    Text poisonMsg;

    private Text[] winTexts;

    private float enemyMsgTimer;
    private float reorientTimer;

    private QuickSlot opt;

    // Use this for initialization
    void Start()
    {
        winTexts = winMessage.GetComponentsInChildren<Text>();
        NoWarning();
        autopilotMsg.enabled = true;
        opt = GameObject.Find("QuickSlot").GetComponent<QuickSlot>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyMsgTimer > 0f)
            enemyMsgTimer -= Time.deltaTime;
        else
        {
            if (enemyMsgTimer < 0f)
                EnemyAway();

            enemyMsgTimer = 0f;
        }

        if (reorientTimer > 0f)
            reorientTimer -= Time.deltaTime;
        else
        {
            if (reorientTimer < 0f)
                NoOrient();

            reorientTimer = 0f;
        }
    }

    #region Msg Functions
    void NoWarning()
    {
        reorientMsg.enabled = false;
        autopilotMsg.enabled = false;
        enemyClose.enabled = false;
        missileInc.enabled = false;
        collectedLoot.enabled = false;
        poisonMsg.enabled = false;
    }
    public void EnemyClose()
    {
        enemyMsgTimer = 10f;
        //Debug.Log("Quick Slot buttons Added");
        //opt.ActivateOption(SystemType.CLOAK);
        //opt.ActivateOption(SystemType.EMP);
        //opt.ActivateOption(SystemType.HYPERDRIVE);
        enemyClose.enabled = true;
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
        reorientTimer = 5f;
        reorientMsg.enabled = true;
    }
    void NoOrient()
    {
        reorientMsg.enabled = false;
    }

    void Poison()
    {
        poisonMsg.enabled = true;
    }
    void NoPoison()
    {
        poisonMsg.enabled = false;
    }
    #endregion

    void Win()
    {
        winMessage.SetActive(true);
        Debug.Log("Won");
        StartCoroutine(WinMessage());
    }

    IEnumerator WinMessage()
    {
        winTexts[2].text = "Returning to Main Menu .";
        yield return new WaitForSeconds(1.0f);
        winTexts[2].text = "Returning to Main Menu . .";
        yield return new WaitForSeconds(1.0f);
        winTexts[2].text = "Returning to Main Menu . . .";
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("MainMenu");
    }
}