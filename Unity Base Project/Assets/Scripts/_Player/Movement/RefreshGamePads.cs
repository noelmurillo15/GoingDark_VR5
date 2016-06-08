using UnityEngine;
using System.Collections;

public class RefreshGamePads : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        GamePadManager.Instance.Refresh();
	}
}
