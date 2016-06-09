using UnityEngine;

public class ExplosionRecycle : MonoBehaviour {

    [SerializeField]
    public float timer = 1f;

    void Initialize()
    {
        gameObject.SetActive(false);
    }

    void OnEnable() {
        Invoke("Deactivate", timer);
    }
    void Deactivate() {
        gameObject.SetActive(false);
    }
}
