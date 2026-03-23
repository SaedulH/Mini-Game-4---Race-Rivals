using CoreSystem;
using System;
using System.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;
using Utilities;

public class CameraZoom : NonPersistentSingleton<CameraZoom>
{
    private CinemachineCamera cinemachineCamera;
    private CinemachinePositionComposer positionComposer;
    private CinemachineConfiner2D confiner2D;
    [field: SerializeField] public PolygonCollider2D Boundary {  get; set; }
    [field: SerializeField] public Transform TrackingTarget { get; private set; }
    private bool trackGroupCentre = false;
    [field: SerializeField] public GameObject GroupCentre { get; private set; }
    [field: SerializeField] public GameObject PlayerOne { get; private set; }
    [field: SerializeField] public GameObject PlayerTwo { get; private set; }

    private float defaultFOV;
    private float targetFOV;
    private float zoomTime;
    private bool isZooming;

    protected override void Awake()
    {
        base.Awake();
        cinemachineCamera = GetComponent<CinemachineCamera>();
        defaultFOV = cinemachineCamera.Lens.FieldOfView;
        positionComposer = gameObject.GetOrAdd<CinemachinePositionComposer>();
        confiner2D = gameObject.GetOrAdd<CinemachineConfiner2D>();
    }
    public async Task SetupCameraMode(TrackContext context, string cameraMode)
    {
        await ResetCameraZoom();
        if (cinemachineCamera == null) return;

        if (Enum.TryParse(cameraMode, out CameraMode parsedCameraMode))
        {
            switch (parsedCameraMode)
            {
                case CameraMode.Fixed:
                default:
                    await SetupFixedCameraMode();
                    break;
                case CameraMode.Dynamic:
                    await SetupDynamicCameraMode(context.PlayerCount);
                    break;
            }
        }
        Debug.Log($"Camera Mode set to: {cameraMode}");
    }

    private async Task SetupFixedCameraMode()
    {
        cinemachineCamera.LookAt = null;
        trackGroupCentre = false;
        if (positionComposer != null)
        {
            positionComposer.enabled = false;

        }
        if (confiner2D != null)
        {
            confiner2D.enabled = false;
        }
        await Task.CompletedTask;
    }

    private async Task SetupDynamicCameraMode(int playerCount)
    {
        if (positionComposer != null)
        {
            positionComposer.enabled = true;
            positionComposer.Lookahead.Enabled = true;
            positionComposer.Lookahead.Time = Constants.DYNAMIC_CAMERA_LOOK_AHEAD_TIME;
            positionComposer.Lookahead.Smoothing = Constants.DYNAMIC_CAMERA_LOOK_AHEAD_SMOOTHING;
        }
        if (confiner2D != null)
        {
            confiner2D.enabled = true;
            if (Boundary != null)
            {
                Boundary.isTrigger = true;
                
            }
        }
        if (playerCount == 1)
        {
            if (PlayerOne != null)
            {
                TrackingTarget = PlayerOne.transform;
            }
        }
        else
        {
            TrackingTarget = GroupCentre.transform;
        }
        confiner2D.InvalidateBoundingShapeCache();
        TrackingTarget.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        cinemachineCamera.LookAt = TrackingTarget;
        trackGroupCentre = true;
        await Task.CompletedTask;
    }

    public async Task ResetCameraZoom()
    {
        if (cinemachineCamera == null)
        {
            GetComponent<CinemachineCamera>();
        }
        defaultFOV = cinemachineCamera.Lens.FieldOfView;
        ResetZoom(0.1f);

        await Task.CompletedTask;
    }

    public void ZoomWithTargetAndDuration(float distance, Transform target, float time)
    {
        targetFOV = defaultFOV - distance; // Zoom in by reducing FOV
        zoomTime = time;
        cinemachineCamera.LookAt = target;
        isZooming = true;
    }

    public void SetZoomByPercentage()
    {

        //targetFOV = 
    }

    private void AdjustConfinerForZoom(float zoomFactor)
    {
        if (confiner2D == null || Boundary == null) return;

        var collider = Boundary as PolygonCollider2D;
        if (collider == null) return;

        //collider.size = originalBoundarySize * zoomFactor;
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
