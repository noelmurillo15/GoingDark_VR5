using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour {


    #region Properties
    [SerializeField]
    private GameObject[] Levels;
    [SerializeField]
    private GameObject[] Windows;
    [SerializeField]
    private GameObject[] Selected;
    [SerializeField]
    private GameObject Accept;
    [SerializeField]
    private GameObject BG;

    private bool rayhit;

    private int range;
    private int layermask;
    private int lastselected;

    private RaycastHit hit;
    private Transform MyTransform;    
    #endregion


    // Use this for initialization
    void Start () {
        lastselected = 0;
        rayhit = true;
        range = 2000;
        layermask = 1 << 15;
        MyTransform = transform;

        CloseAllPanels();
    }

    public void AcceptLevel(string diff)
    {
        if (lastselected != 0)
        {
            PlayerPrefs.SetString("Difficulty", diff);
            string selected = "Level";
            selected += lastselected.ToString();
            SceneManager.LoadScene(selected);
        }
    }
	
	// Update is called once per frame
	void LateUpdate () {
        rayhit = false;

        if (Physics.Raycast(MyTransform.position, MyTransform.forward, out hit, range, layermask))
            if (hit.collider.CompareTag("Star"))
                rayhit = true;

        if (rayhit)
        {
            for (int x = 0; x < Levels.Length; x++)
            {
                if (hit.transform.gameObject == Levels[x])
                {
                    Windows[x].SetActive(true);
                    if (x != 0)
                    {
                        lastselected = x;
                        Accept.SetActive(true);
                    }
                    BG.SetActive(true);
                    Selected[x].SetActive(true);
                }
            }
        }
        else
            CloseAllPanels();
    }

    void CloseAllPanels()
    {
        if (BG.activeSelf == true)
        {
            BG.SetActive(false);
            for (int x = 0; x < Levels.Length; x++)
            {
                Windows[x].SetActive(false);
                Selected[x].SetActive(false);
            }
            Accept.SetActive(false);
        }
    }
}