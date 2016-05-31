using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {

    float duration = .25f;
    float speed = 10.0f;
    float magnitude = .1f;
    Transform MyTransform;
    Vector3 originalpos;

    void Update()
    {
        //Press F on the keyboard to simulate the effect
        if (Input.GetKeyDown(KeyCode.S))
        {
            PlayShake();
        }
    }

    //This function is used outside (or inside) the script
    public void PlayShake()
    {
        Debug.Log("Camera Shake");
        MyTransform = transform;
        StopAllCoroutines();
        StartCoroutine("Shake");
    }

    private IEnumerator Shake()
    {
        float elapsed = 0.0f;

        Vector3 originalCamPos = MyTransform.localPosition;
        float randomStart = Random.Range(-1000.0f, 1000.0f);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float percentComplete = elapsed / duration;

            float damper = 1.0f - Mathf.Clamp(1.5f * percentComplete - 1.0f, 0.0f, 1.0f);
            float alpha = randomStart + speed * percentComplete;

            float x = Mathf.PerlinNoise(alpha, 0.0f) * 2.0f - 1.0f;
            float y = Mathf.PerlinNoise(0.0f, alpha) * 2.0f - 1.0f;

            x *= magnitude * damper;
            y *= magnitude * damper;

            MyTransform.localPosition = new Vector3(x, y, originalCamPos.z);

            yield return 0;
        }
        Debug.Log("Camera Stopped Shake");
        MyTransform.localPosition = Vector3.zero;
    }
}
