using UnityEngine;
using GD.Core.Enums;

public class Decoy : MonoBehaviour {

    #region Properties
    public bool Activated { get; private set; }
    public float Cooldown { get; private set; }

    public int numDecoys;
    private GameObject cam;
    public GameObject decoy;
    #endregion

    // Use this for initialization
    void Start() {
        numDecoys = 5;
        cam = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update() {

    }

    public void LeaveDecoy()
    {
        if (numDecoys > 0)
        {
            numDecoys--;
            Instantiate(decoy, cam.transform.position, cam.transform.localRotation);
        }
    }
}