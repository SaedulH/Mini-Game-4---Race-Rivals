using System;
using System.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;
using Utilities;

public class CameraZoom : NonPersistentSingleton<CameraZoom>
{
    private CinemachineCamera cinemachineCamera;

    [field: SerializeField] public GameObject PlayerOne { get; private set; }
    private Rigidbody2D _rb1;
    [field: SerializeField] public GameObject PlayerTwo { get; private set; }
    private Rigidbody2D _rb2;

    private float defaultFOV;
    private float targetFOV;
    private float zoomTime;
    private bool isZooming;

    protected override void Awake()
    {
        base.Awake();
        cinemachineCamera = GetComponent<CinemachineCamera>();
        defaultFOV = cinemachineCamera.Lens.FieldOfView; // Store default FOV
    }
    public async Task SetupCameraMode(string cameraMode)
    {
        if (Enum.TryParse(cameraMode, out CameraMode parsedCameraMode))
        {
            switch (parsedCameraMode)
            {
                case CameraMode.Fixed:
                default:
                    await ResetCameraZoom();
                    await EnableTargetGroupTracking(false);
                    break;
                case CameraMode.Dynamic:
                    await EnableTargetGroupTracking(true);
                    break;
            }
        }
        else
        {
            await ResetCameraZoom();
        }
        Debug.Log($"Camera Mode set to: {cameraMode}");
    }

    private async Task EnableTargetGroupTracking(bool enabled)
    {
        //if (cinemachineGroupFraming == null) return;

        //cinemachineGroupFraming.enabled = enabled;

        await Task.CompletedTask;
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
        targetFOV = defaultFOV - distance; // Zoom in by reducing FOV
        zoomTime = time;
        cinemachineCamera.LookAt = target;
        isZooming = true;
    }

    public void ResetZoom(float resetTime = 0.5f)
    {
        targetFOV = defaultFOV;

        zoomTime = (resetTime > 0f) ? resetTime : zoomTime; // Use last zoom time if not specified
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
