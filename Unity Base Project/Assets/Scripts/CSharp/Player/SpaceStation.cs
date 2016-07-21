using UnityEngine;

public class SpaceStation : MonoBehaviour
{
    #region Properties
    private float repairTimer;
    private AudioSource sound;
    private SystemManager systemManager;
    #endregion


    // Use this for initialization
    void Start()
    {
        repairTimer = 0f;
        sound = GetComponent<AudioSource>();
        systemManager = GameObject.FindGameObjectWithTag("Systems").GetComponent<SystemManager>();
    }

    void Update()
    {
        if (repairTimer > 0)
            repairTimer -= Time.deltaTime;
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.transform.tag == "Player" && repairTimer <= 0f)
        {
            sound.Play();
            repairTimer = 60f;
            systemManager.FullSystemRepair();
        }
    }
}
