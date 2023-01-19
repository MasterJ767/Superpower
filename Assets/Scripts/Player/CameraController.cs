using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
public class CameraController : MonoBehaviour {
    [Header("Settings")]
    public Transform followTarget;
    public float rotationSpeed = 2.0f;
    public float minfollowDistance = 2.0f;
    public float maxfollowDistance = 5.0f;
    public Vector2 framingOffset;
    public float minVerticalAngle = -45.0f;
    public float maxVerticalAngle = 45.0f;
    public bool invertX;
    public bool invertY;
    public bool invertZoom;

    [HideInInspector] public float rotationX;
    [HideInInspector] public float rotationY;
    private float zoom = 0f;

    private void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() {
        float invertXVal = invertX ? -1 : 1; 
        float invertYVal = invertY ? -1 : 1; 
        rotationX += Input.GetAxis("Mouse Y") * rotationSpeed * invertXVal;
        rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);
        rotationY += Input.GetAxis("Mouse X") * rotationSpeed * invertYVal;

        float invertZoomVal = invertZoom ? -1 : 1;
        float zoomDelta = Input.GetAxis("Mouse ScrollWheel") * invertZoomVal;
        zoom = Mathf.Clamp01(zoom + zoomDelta);
        float followDistance = Mathf.Lerp(maxfollowDistance, minfollowDistance, zoom); 
        float zoomHeight = Mathf.Lerp(0, 0.1f, zoom);

        Quaternion targetRotation = Quaternion.Euler(rotationX, rotationY, 0);
        Vector3 focusPosition = followTarget.position + new Vector3(framingOffset.x, framingOffset.y, 0);
        transform.position = focusPosition - (targetRotation * new Vector3(0, zoomHeight, followDistance));
        transform.rotation = targetRotation;   
    }
}
}
