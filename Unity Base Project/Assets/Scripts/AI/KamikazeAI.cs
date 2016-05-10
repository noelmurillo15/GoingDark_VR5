using UnityEngine;

[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(CharacterController))]
public class KamikazeAI : MonoBehaviour {
    //**        Attach to Enemy     **//

    private float padding;


    // Use this for initialization
    void Start()
    {
        padding = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (padding > 0f)
            padding -= Time.deltaTime;        
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.CompareTag("Player") && padding <= 0f)
        {
            padding = 1f;
            Debug.Log("Droid Has Hit");
            hit.transform.SendMessage("Hit");
            hit.transform.SendMessage("EMPHit");
            transform.SendMessage("Kill");
        }
    }
}