using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
public class PlayerMovement : MonoBehaviour {
    [Header("Camera")]
    public CameraController cameraController;

    [Header("Movement Speed")]
    public float walkSpeed = 6.0f;
    public float runSpeed = 12.5f;
    public float walkAccelerationTime = 0.5f;
    public float runAccelerationTime = 3.75f;
    public float rotationSpeed = 500.0f;

    [Header("Jumping")]
    public float jumpHeight = 3.0f;
    public float groundCheckDistance = 0.3f;
    public Vector3 groundCheckOffset;
    public LayerMask environmentLayer;
    public float gravity = -9.81f;

    [Header("Stamina")]
    public float costToRunPerSecond = 30.0f;
    public float costToJump = 80.0f;
    
    private Animator animator;
    private int animatorMoveSpeed;

    private CharacterController characterController;
    private Quaternion targetRotation;

    private Statistics.Health health;
    private Statistics.Stamina stamina;

    private bool isGrounded;
    private bool isFalling;
    private bool isJumping;
    private bool isRunning;
    private bool isAttacking;
    private bool isInteracting;

    private float speedHorizontal = 4.5f;
    private float speedVertical;
    private Vector3 externalForces;
    private float fallDistance = 0.0f;
    private float fallInitialY = float.MinValue;

    private void Awake() {
        animator = GetComponent<Animator>();
        animatorMoveSpeed = Animator.StringToHash("MoveSpeed");
        characterController = GetComponent<CharacterController>();
        health = GetComponent<Statistics.Health>();
        stamina = GetComponent<Statistics.Stamina>();
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = new Color(1.0f, 0.5f, 0.0f, 1.0f);
        Gizmos.DrawWireSphere(transform.TransformPoint(groundCheckOffset), groundCheckDistance);
    }

    private void Update() {
        GroundCheck();
        FallCheck();
        RunCheck();
        JumpCheck();
        MovePlayer();
        ResolveForces();
    }

    private void LateUpdate() {
        isInteracting = animator.GetBool("IsInteracting");
        isJumping = animator.GetBool("IsJumping");
        isAttacking = animator.GetBool("IsAttacking");
        animator.SetBool("IsGrounded", isGrounded);
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
                health.Damage(CalculateFallDamage());
                fallDistance = 0.0f;
                fallInitialY = float.MinValue;
            }
        }
        else {
            speedVertical += gravity * Time.deltaTime;
            isFalling = true;

            RaycastHit hit;
            Physics.Raycast(transform.TransformPoint(groundCheckOffset), -transform.up, out hit, Int32.MaxValue, environmentLayer);

            if (!isJumping && !isInteracting && fallDistance < 0.01f) {
                PlayTargetAnimation("Falling", true);
                fallDistance = hit.distance;
                if (fallInitialY <= float.MinValue + float.Epsilon) { fallInitialY = transform.position.y; }
            }

            if (hit.distance > fallDistance) { fallDistance = hit.distance; }
        }
    }

    private float CalculateFallDamage() {
        float threshold = jumpHeight * 1.75f;
        return fallDistance <= threshold || fallInitialY - transform.position.y <= threshold ? 0.0f : (fallDistance - threshold) * 8.25f;
    }

    private void RunCheck() {            
        isRunning = isGrounded && !isAttacking && Input.GetButton("Run") && stamina.ExpendQuery(costToRunPerSecond * Time.deltaTime) > 0;
        if (isRunning) { stamina.Expend(costToRunPerSecond * Time.deltaTime); }
    }

    private void JumpCheck() {
        if (isGrounded && !isAttacking && Input.GetButtonDown("Jump") && stamina.ExpendQuery(costToJump) > 0) {
            animator.SetBool("IsJumping", true);
            PlayTargetAnimation("Jump", false);

            stamina.Expend(costToJump);
            AddForce(Vector3.up, -gravity * jumpHeight);
            fallInitialY = transform.position.y;
        }
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
        animator.SetBool("IsInteracting", interactingState);
        animator.CrossFade(targetAnimation, 0.2f);
    }

    public void PlayAttackAnimation(string targetAnimation) {
        isAttacking = true;
        animator.SetBool("IsAttacking", true);
        PlayTargetAnimation(targetAnimation + "Up", true);
        PlayTargetAnimation(targetAnimation + "Low", true);
    }

    public void AddForce(Vector3 direction, float magnitude) {
        externalForces += direction.normalized * magnitude;
    }

    private void ResolveForces() {
        if (externalForces.magnitude > 0.2f) { characterController.Move(externalForces * Time.deltaTime); }
        externalForces = Vector3.Lerp(externalForces, Vector3.zero, 5 * Time.deltaTime);
    }
}
}