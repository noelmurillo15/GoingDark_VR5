using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{

    [SerializeField]
    GameObject mMenu;

    // Use this for initialization
    void Start() {
        if (mMenu == null)
            mMenu = GameObject.Find("Tactician_Menu");
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Collision
    public void OnTriggerEnter(Collider col) {
        if (col.name == "CenterEyeAnchor")
            mMenu.SetActive(true);
    }

    public void OnTriggerExit(Collider col) {
        if (col.name == "CenterEyeAnchor")
            mMenu.SetActive(false);
    }
    #endregion
}