using UnityEngine;
using UnityEngine.UI;
using MovementEffects;
using GoingDark.Core.Enums;
using System.Collections.Generic;

public class DisplayCooldown : MonoBehaviour {

    private Text text;
    private SystemType type;
    private SystemManager system;


	void Start () {
        system = GameObject.Find("Devices").GetComponent<SystemManager>();
        type = GetComponentInParent<QuickSlot>().Type;
        text = GetComponent<Text>();

        //   Start Coroutine
        Timing.RunCoroutine(UpdateCooldowns());
	}

    #region Coroutine
    private IEnumerator<float> UpdateCooldowns()
    {
        while (true)
        {
            CooldownCheck();
            yield return Timing.WaitForSeconds(1);
        }
    }
    private void CooldownCheck()
    {
        int num = system.GetSystemCooldown(type);
        if (num == -1)
        {
            text.color = Color.grey;
            text.text = "System Unavailable";
        }
        else if (num == -10)
        {
            text.color = Color.grey;
            text.text = "System Offline";
        }
        else if (num == 0)
        {
            text.color = Color.green;
            text.text = "System Ready";
        }
        else
        {
            text.color = Color.red;
            text.text = num.ToString();
        }
    }
    #endregion
}
