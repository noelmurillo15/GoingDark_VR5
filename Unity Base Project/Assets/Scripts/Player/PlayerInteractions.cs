using UnityEngine;

public class PlayerInteractions : MonoBehaviour {

    private float transition;
    private float cancelTimer;

    [SerializeField]
    GameObject mMenu;
    //private PlayerData playerData;
    //private EMP emp;

    // Use this for initialization
    void Start()
    {
        transition = 0.0f;
        cancelTimer = 0.0f;

        if(mMenu == null)
            mMenu = GameObject.Find("Tactician_Menu");
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Collision
    public void OnTriggerEnter(Collider col) {
        if (col.name == "bone3") {
            if (col.transform.parent.name == "leftIndex" || col.transform.parent.name == "rightIndex")
                mMenu.SetActive(!mMenu.activeSelf);            
        }
    }
    #endregion
}