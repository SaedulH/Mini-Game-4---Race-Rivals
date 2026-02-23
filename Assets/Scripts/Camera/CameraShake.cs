using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using Utilities;

public class CameraShake : NonPersistentSingleton<CameraShake>
{
    private CinemachineCamera cinemachineCamera;
    private Coroutine _shakeCoroutine = null;
    protected override void Awake()
    {
        base.Awake();
        cinemachineCamera = GetComponent<CinemachineCamera>();
    }

    public void ShakeCamera(float intensity, float duration)
    {
        if (cinemachineCamera == null) return;

        if(_shakeCoroutine != null)
        {
            StopCoroutine(_shakeCoroutine);
            cinemachineCamera.transform.localPosition = new Vector3(0f, 0f, -5f);
        }
        StartCoroutine(ShakeCameraCoroutine(intensity, duration));
    }

    private IEnumerator ShakeCameraCoroutine(float intensity, float duration)
    {

        Vector3 originalPosition = new Vector3(0f, 0f, -5f);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float xOffset = Random.Range(-0.5f, 0.5f) * intensity;
            float yOffset = Random.Range(-0.5f, 0.5f) * intensity;
            cinemachineCamera.transform.localPosition = originalPosition + new Vector3(xOffset, yOffset, 0f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cinemachineCamera.transform.localPosition = originalPosition;
    }
}