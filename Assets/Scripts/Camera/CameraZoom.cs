using System.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;
using Utilities;

public class CameraZoom : NonPersistentSingleton<CameraZoom>
{
    private CinemachineCamera cinemachineCamera;

    private float defaultFOV;
    private float startFOV;
    private float targetFOV;
    private float zoomTime;
    private bool isZooming;
    private float zoomTimeElapsed = 0f;

    protected override void Awake()
    {
        base.Awake();
        cinemachineCamera = GetComponent<CinemachineCamera>();
        defaultFOV = cinemachineCamera.Lens.FieldOfView; // Store default FOV
    }

    public async Task ResetCameraZoom()
    {
        cinemachineCamera = GetComponent<CinemachineCamera>();
        defaultFOV = cinemachineCamera.Lens.FieldOfView;
        ResetZoom(0.1f);

        await Task.CompletedTask;
    }

    public void Zoom(float distance, Transform target, float time)
    {
        startFOV = cinemachineCamera.Lens.FieldOfView; // Start from current FOV
        targetFOV = defaultFOV - distance; // Zoom in by reducing FOV
        zoomTime = time;
        zoomTimeElapsed = 0f;
        cinemachineCamera.LookAt = target;
        isZooming = true;
    }

    public void ResetZoom(float resetTime = 0.5f)
    {
        startFOV = cinemachineCamera.Lens.FieldOfView; // Start from current FOV
        targetFOV = defaultFOV;

        zoomTime = (resetTime > 0f) ? resetTime : zoomTime; // Use last zoom time if not specified
        zoomTimeElapsed = 0f;
        isZooming = true;
    }

    void Update()
    {
        if (isZooming)
        {
            // Smooth transition using Lerp
            cinemachineCamera.Lens.FieldOfView = Mathf.Lerp(cinemachineCamera.Lens.FieldOfView, targetFOV, 5f * Time.deltaTime);

            // Timer-based zoom reset
            if (zoomTime > 0f)
            {
                zoomTime -= Time.deltaTime;
                if (zoomTime <= 0f)
                {
                    ResetZoom();
                }
            }

            // Stop zooming when close enough to target
            if (Mathf.Abs(cinemachineCamera.Lens.FieldOfView - targetFOV) < 0.1f)
            {
                isZooming = false;
            }
        }
    }
}
