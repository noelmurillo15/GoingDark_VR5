using UnityEngine;
using UnityEngine.UI;
using GoingDark.Core.Enums;
using MovementEffects;
using System.Collections.Generic;

public class MissionLog : MonoBehaviour
{
    // message shit, fuck it
    [SerializeField]
    private GameObject m_pMissionMessage;
    [SerializeField]
    private Text mText;
    // Use this for initialization

    #region Messages

    public void Failed(Mission mission)
    {
        m_pMissionMessage.SetActive(true);
        mText.text = "Mission '" + mission.missionName + "' failed";
        Timing.RunCoroutine(Messages());
    }

    public void Completed(Mission mission)
    {
        mText.text = "Mission '" + mission.missionName + "' is completed";
        Timing.RunCoroutine(Messages());
        m_pMissionMessage.SetActive(true);
    }

    IEnumerator<float> Messages()
    {
        yield return Timing.WaitForSeconds(5.0f);
        m_pMissionMessage.SetActive(false);
    }
    #endregion


}
