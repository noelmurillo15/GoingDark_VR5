using UnityEngine;

public class DebuffManager : MonoBehaviour {

    private GameObject stunned;
    private PlayerMovement move; 

	// Use this for initialization
	void Start () {
        move = GetComponentInParent<PlayerMovement>();
        stunned = transform.GetChild(0).gameObject;
	}
	
	public void Stunned(float _timer)
    {
        move.PlayerStunned();
        stunned.SetActive(true);
        stunned.transform.localRotation = move.transform.localRotation;
        Invoke("RemoveStun", _timer);
    }

    void RemoveStun()
    {
        stunned.SetActive(false);
    }
}
