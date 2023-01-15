using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
public class PlayerController : MonoBehaviour {
    [Header("Camera")]
    public CameraController cameraController;

    [Header("Movement Speed")]
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 500.0f;

    [Header("Jumping")]
    public float groundCheckDistance = 0.3f;
    public Vector3 groundCheckOffset;
    public LayerMask environmentLayer;
    public float gravity = -9.81f;
    
    private Animator animator;
    private CharacterController characterController;
    private Quaternion targetRotation;

    private bool isGrounded;
    private float speedY;

    private void Awake() {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = new Color(1.0f, 0.5f, 0.0f, 1.0f);
        Gizmos.DrawWireSphere(transform.TransformPoint(groundCheckOffset), groundCheckDistance);
    }

    private void Update() {
        GroundCheck();
        FallCheck();
        MovePlayer();
    }

    private void GroundCheck() {
        isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckDistance, environmentLayer);
    }

    private void FallCheck() {
        if (isGrounded) {
            speedY = -1.0f;
        }
        else {
            speedY += gravity * Time.deltaTime;
        }
    }

    private void MovePlayer() {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float moveDelta = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

        Vector3 moveInput = new Vector3(horizontal, 0, vertical).normalized;
        Vector3 moveDirection = cameraController.PlanarRotation * moveInput;

        Vector3 velocity = CalculateVelocity(moveDirection);
        velocity.y = speedY;

        characterController.Move(velocity * Time.deltaTime);

        if (moveDelta > 0) {
            targetRotation = Quaternion.LookRotation(moveDirection);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        animator.SetFloat("MoveSpeed", moveDelta, 0.2f, Time.deltaTime);
    }

    private Vector3 CalculateVelocity(Vector3 moveDirection) {
        return moveDirection * moveSpeed;
    }
}
}