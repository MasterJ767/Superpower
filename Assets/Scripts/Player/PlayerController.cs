using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
public class PlayerController : MonoBehaviour
{
    public CameraController cameraController;
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 500.0f;

    private Animator animator;
    private Quaternion targetRotation;

    private void Awake(){
        animator = GetComponent<Animator>();
    }

    private void Update(){
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float moveDelta = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

        Vector3 moveInput = new Vector3(horizontal, 0, vertical).normalized;
        Vector3 moveDirection = cameraController.PlanarRotation * moveInput;

        if (moveDelta > 0) {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            targetRotation = Quaternion.LookRotation(moveDirection);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        animator.SetFloat("MoveSpeed", moveDelta, 0.2f, Time.deltaTime);
    }
}
}