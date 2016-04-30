using UnityEngine;
using UnityEngine.UI;

public class LootCounter : MonoBehaviour {

    [SerializeField]
    Text textCount;
    public int lootCounter;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        textCount.text = lootCounter.ToString();
	}
}
