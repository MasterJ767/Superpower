using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
public class CameraController : MonoBehaviour {
    [Header("Settings")]
    public Transform followTarget;
    public float movementSpeed = 6f;
    public float rotationSpeed = 2.0f;
    public float minfollowDistance = 2.0f;
    public float maxfollowDistance = 5.0f;
    public Vector2 framingOffset;
    public float minVerticalAngle = -45.0f;
    public float maxVerticalAngle = 45.0f;
    public bool invertX;
    public bool invertY;
    public bool invertZoom;
    public LayerMask environmentLayer;
    public LayerMask playerLayer;

    [HideInInspector] public float rotationX;
    [HideInInspector] public float rotationY;
    private float zoom = 0f;
    [HideInInspector] public bool isRewinding;
    private Vector3 currentVelocity;

    private void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() {
        if (!isRewinding) {
            float invertXVal = invertX ? -1 : 1; 
            float invertYVal = invertY ? -1 : 1; 
            rotationX += Input.GetAxis("Mouse Y") * rotationSpeed * invertXVal;
            rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);
            rotationY += Input.GetAxis("Mouse X") * rotationSpeed * invertYVal;
            if (rotationY < 0.0f) { rotationY += 360.0f; }
            if (rotationY > 360.0f) { rotationY -= 360.0f; }

            float invertZoomVal = invertZoom ? -1 : 1;
            float zoomDelta = Input.GetAxis("Mouse ScrollWheel") * invertZoomVal;
            zoom = Mathf.Clamp01(zoom + zoomDelta);
            float followDistance = Mathf.Lerp(maxfollowDistance, minfollowDistance, zoom); 
            float zoomHeight = Mathf.Lerp(0, 0.1f, zoom);

            Quaternion targetRotation = Quaternion.Euler(rotationX, rotationY, 0);
            transform.rotation = targetRotation;

            Vector3 focusPosition = followTarget.position + new Vector3(framingOffset.x, framingOffset.y, 0);
            Vector3 idealPosition = focusPosition - (targetRotation * new Vector3(0, zoomHeight, followDistance));

            Vector3 displacement = focusPosition - idealPosition;
            float distance = displacement.magnitude;
            Vector3 direction = displacement.normalized;

            RaycastHit hit;
            if (Physics.Raycast(focusPosition, -direction, out hit, distance, environmentLayer)) { 
                Vector3 compromisePosition = focusPosition - (direction * hit.distance);
                RaycastHit hit2;
                if (Physics.Raycast(idealPosition, direction, out hit2, distance, environmentLayer | playerLayer)) { 
                    if (distance - hit.distance - hit2.distance < 0) {
                        float interpolatedDistance = ((distance - hit2.distance) + hit.distance) * 0.5f;
                        idealPosition = focusPosition - (direction * interpolatedDistance);
                    }
                    else {
                        idealPosition = compromisePosition;
                    }
                }
            }
            
            transform.position = idealPosition;
        }
    }
}
}
