using UnityEngine;
using GD.Core.Enums;

public class DecoySystem : MonoBehaviour {

    #region Properties
    public bool Activated { get; private set; }
    public float Cooldown { get; private set; }

    public int Count { get; private set; }
    private GameObject cam;
    public GameObject decoy;

    private SystemsManager manager;
    #endregion

    // Use this for initialization
    void Start() {
        Count = 5;
        cam = GameObject.FindGameObjectWithTag("Player");
        manager = GameObject.FindGameObjectWithTag("Systems").GetComponent<SystemsManager>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKey(KeyCode.D))
            manager.ActivateSystem(SystemType.DECOY);
    }

    public void Activate()
    {
        if (Count > 0)
        {
            Count--;
            Instantiate(decoy, cam.transform.position, cam.transform.localRotation);
        }
    }
}