using UnityEngine;

public class ParticleDespawn : MonoBehaviour {
	public float Duration;
	public float timer;

	void Update () {
		timer += Time.deltaTime;
		float durration = gameObject.GetComponent<ParticleSystem>().duration;
		Duration = durration;
			Destroy(gameObject,Duration);
	}
}
