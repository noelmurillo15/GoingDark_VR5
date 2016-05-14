using UnityEngine;
using GD.Core.Enums;

public class QuickSlot : MonoBehaviour
{

    public bool Active { get; set; }
    public int MaxButtons;
    public int currButtons;
    private float xOffset;

    //public float[] timers;
    public Vector3[] positions;
    public GameObject[] buttons;
    public GameObject[] activeButtons;

    public GameObject background;    


    // Use this for initialization
    void Start()
    {
        currButtons = 0;
        xOffset = 0.3f;
        Active = true;
        MaxButtons = 5;
        background = transform.GetChild(5).gameObject;
        //timers = new float[MaxButtons];
        positions = new Vector3[MaxButtons];
        buttons = new GameObject[MaxButtons];
        activeButtons = new GameObject[MaxButtons];
        for (int x = 0; x < MaxButtons; x++)
        {
            buttons[x] = transform.GetChild(x).gameObject;
            positions[x] = buttons[x].transform.localPosition;            
        }
        ActivateOption(SystemType.MISSILES);
        ActivateOption(SystemType.HYPERDRIVE);
        ActivateOption(SystemType.DECOY);
        ActivateOption(SystemType.CLOAK);
        ActivateOption(SystemType.EMP);
    }

    // Update is called once per frame
    void Update()
    {
        //for (int x = 0; x < MaxButtons; x++)
        //{
        //    if (timers[x] > 0f)
        //        timers[x] -= Time.deltaTime;
        //    else
        //    {
        //        if(timers[x] < 0f)
        //        {
        //            currButtons--;
        //            activeButtons[currButtons].SetActive(false);
        //        }
        //        timers[x] = 0f;
        //    }
        //}
    }

    public void ActivateOption(SystemType type)
    {
        if (currButtons > MaxButtons)
            return;

        switch (type)
        {
            case SystemType.EMP:
                activeButtons[currButtons] = buttons[4];
                activeButtons[currButtons].transform.localPosition = positions[currButtons];
                //activeButtons[currButtons].SetActive(true);
                //timers[currButtons] = 10f;
                break;
            case SystemType.CLOAK:
                activeButtons[currButtons] = buttons[0];
                activeButtons[currButtons].transform.localPosition = positions[currButtons];
                //activeButtons[currButtons].SetActive(true);
                //timers[currButtons] = 10f;
                break;
            case SystemType.DECOY:
                activeButtons[currButtons] = buttons[3];
                activeButtons[currButtons].transform.localPosition = positions[currButtons];
                //activeButtons[currButtons].SetActive(true);
                //timers[currButtons] = 10f;
                break;
            case SystemType.MISSILES:
                activeButtons[currButtons] = buttons[2];
                activeButtons[currButtons].transform.localPosition = positions[currButtons];
                //activeButtons[currButtons].SetActive(true);
                //timers[currButtons] = 10f;
                break;
            case SystemType.HYPERDRIVE:
                activeButtons[currButtons] = buttons[1];
                activeButtons[currButtons].transform.localPosition = positions[currButtons];
                //activeButtons[currButtons].SetActive(true);
                //timers[currButtons] = 10f;
                break;
            default:
                break;
        }        
        currButtons++;
    }

    public void CloseSettings()
    {
        Active = false;
    }
}