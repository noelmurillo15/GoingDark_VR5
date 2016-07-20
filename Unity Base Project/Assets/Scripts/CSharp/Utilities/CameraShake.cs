using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {

    float duration = .15f;
    float speed = .01f;
    Transform MyTransform;
    Vector3 originalpos;

    void Start()
    {
        MyTransform = transform;
        originalpos = MyTransform.localPosition;
    }

    public void PlayShake()
    {
        StopAllCoroutines();
        StartCoroutine("Shake");
    }

    private IEnumerator Shake()
    {
        float elapsed = 0.0f;

        Vector3 originalCamPos = originalpos;
        float randomStart = Random.Range(-.1f, .1f);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float percentComplete = elapsed / duration;

            float damper = Mathf.Clamp(percentComplete, 0f, .1f);
            float alpha = randomStart + speed * percentComplete;

            float x = Mathf.PerlinNoise(alpha, 0.0f);
            float y = Mathf.PerlinNoise(0.0f, alpha);

            x *= damper * .000000001f;
            y *= damper * .000000001f;

            MyTransform.localPosition = new Vector3(x, y, originalCamPos.z);

            yield return 0;
        }
        MyTransform.localPosition = originalpos;
    }
}
