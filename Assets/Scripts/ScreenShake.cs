using System;
using System.Collections;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    Vector3 _originalPos = new Vector3();

    private void Awake()
    {
        _originalPos = transform.localPosition;
        EventManager.current.OnScreenShake += OnScreenShake;
    }

    private void OnDisable()
    {
        EventManager.current.OnScreenShake -= OnScreenShake;
    }

    private void OnScreenShake(float duration, float magnitude)
    {
        StartCoroutine(ShakeCamera(duration, magnitude));
    }

    IEnumerator ShakeCamera(float duration, float magnitude)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * magnitude;
            float y = UnityEngine.Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, _originalPos.z);

            elapsedTime += Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }

        transform.localPosition = _originalPos;
    }
}
