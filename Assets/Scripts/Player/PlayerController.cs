using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
public class PlayerController : MonoBehaviour {
    [Header("Camera")]
    public CameraController cameraController;

    [Header("Movement Speed")]
    public float walkSpeed = 6.0f;
    public float runSpeed = 12.5f;
    public float walkAccelerationTime = 0.5f;
    public float runAccelerationTime = 3.75f;
    public float rotationSpeed = 500.0f;

    [Header("Jumping")]
    public float groundCheckDistance = 0.3f;
    public Vector3 groundCheckOffset;
    public LayerMask environmentLayer;
    public float gravity = -9.81f;
    
    private Animator animator;
    private int animatorMoveSpeed;

    private CharacterController characterController;
    private Quaternion targetRotation;

    private bool isGrounded;
    private bool isFalling;
    private bool isRunning;
    private bool isInteracting;
    private float speedHorizontal = 4.5f;
    private float speedVertical;
    private float fallDistance = 0.0f;

    private void Awake() {
        animator = GetComponent<Animator>();
        animatorMoveSpeed = Animator.StringToHash("MoveSpeed");
        characterController = GetComponent<CharacterController>();
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = new Color(1.0f, 0.5f, 0.0f, 1.0f);
        Gizmos.DrawWireSphere(transform.TransformPoint(groundCheckOffset), groundCheckDistance);
    }

    private void Update() {
        GroundCheck();
        FallCheck();
        RunCheck();
        MovePlayer();
    }

    private void LateUpdate() {
        isInteracting = animator.GetBool("IsInteracting");
    }

    private void GroundCheck() {
        isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckDistance, environmentLayer);
    }

    private void FallCheck() {
        if (isGrounded) {
            speedVertical = -1.0f;
            if (isFalling) {
                PlayTargetAnimation("Land", true);
                isFalling = false;

                // Fall damage calculation here
                fallDistance = 0.0f;
            }
        }
        else {
            speedVertical += gravity * Time.deltaTime;
            if (!isInteracting && fallDistance < 0.01f) {
                PlayTargetAnimation("Falling", true);
                isFalling = true;

                RaycastHit hit;
                Physics.Raycast(transform.TransformPoint(groundCheckOffset), -transform.up, out hit, Int32.MaxValue, environmentLayer);
                fallDistance = hit.distance;
            }
        }
    }

    private void RunCheck() {            
        isRunning = isGrounded && !isRunning && Input.GetButton("Run");
    }

    private void MovePlayer() {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float moveDelta = Mathf.Abs(horizontal) + Mathf.Abs(vertical);

        Vector3 velocity = new Vector3(0.0f, speedVertical, 0.0f);
        if (moveDelta > 0.001) {
            Vector3 moveInput = new Vector3(horizontal, 0.0f, vertical).normalized;
            Vector3 moveDirection = cameraController.PlanarRotation * moveInput;
            velocity += CalculateHorizontalVelocity(moveDirection);
            targetRotation = Quaternion.LookRotation(moveDirection);
        }
        else {
            speedHorizontal = 4.5f;
            if (!isInteracting) { animator.SetFloat(animatorMoveSpeed, 0.0f, 0.2f, Time.deltaTime); }
        }
        characterController.Move(velocity * Time.deltaTime);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private Vector3 CalculateHorizontalVelocity(Vector3 moveDirection) {
        float targetSpeed = isRunning ? runSpeed : walkSpeed;
        float sign = targetSpeed >= speedHorizontal ? 1 : -1;
        float accelerationTime = isRunning || speedHorizontal > walkSpeed ? runAccelerationTime : walkSpeed;
        float acceleration = sign * (targetSpeed / accelerationTime);
        speedHorizontal += (acceleration * Time.deltaTime);
        speedHorizontal = Mathf.Max(speedHorizontal, 0.001f);
        float speedRatio = Mathf.Clamp01(speedHorizontal / (runSpeed * 2.0f));
        if (!isInteracting) { animator.SetFloat(animatorMoveSpeed, speedRatio, 0.2f, Time.deltaTime); }
        return moveDirection * speedHorizontal;
    }

    private void PlayTargetAnimation(string targetAnimation, bool interactingState) {
        animator.SetBool("isInteracting", interactingState);
        animator.CrossFade(targetAnimation, 0.2f);
    }
}
}