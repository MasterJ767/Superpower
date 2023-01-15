using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
public class CameraController : MonoBehaviour {
    [Header("Settings")]
    public Transform followTarget;
    public float rotationSpeed = 2.0f;
    public float followDistance;
    public Vector2 framingOffset;
    public float minVerticalAngle = -45.0f;
    public float maxVerticalAngle = 45.0f;
    public bool invertX;
    public bool invertY;

    private float rotationX;
    private float rotationY;

    public Quaternion PlanarRotation => Quaternion.Euler(0, rotationY, 0);

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

        Quaternion targetRotation = Quaternion.Euler(rotationX, rotationY, 0);

        Vector3 focusPosition = followTarget.position + new Vector3(framingOffset.x, framingOffset.y, 0);
        transform.position = focusPosition - (targetRotation * new Vector3(0, 0, followDistance));
        transform.rotation = targetRotation;
    }
}
}
