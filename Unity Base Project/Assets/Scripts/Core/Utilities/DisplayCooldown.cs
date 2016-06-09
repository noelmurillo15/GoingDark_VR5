using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GoingDark.Core.Enums;

public class DisplayCooldown : MonoBehaviour {

    private Text text;
    private SystemType type;
    private SystemManager system;


	void Start () {
        type = GetComponentInParent<QuickSlot>().Type;
        text = GetComponent<Text>();
        system = GameObject.Find("Devices").GetComponent<SystemManager>();

        StartCoroutine("UpdateCooldowns");
	}

    #region Coroutine
    private IEnumerator UpdateCooldowns()
    {
        while (true)
        {
            CooldownCheck();
            yield return new WaitForSeconds(1);
        }
    }
    private void CooldownCheck()
    {
        if (system.GetSystemStatus(type))
            text.text = "Cooldown : " + system.GetSystemCooldown(type);        
        else
            text.text = "System Offline";
    }
    #endregion
}
