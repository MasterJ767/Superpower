using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy {
public class EnemyController : MonoBehaviour
{
    [Header("Movement Speed")]
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 500.0f;

    [Header("AI")]
    public float visionRadius = 25.0f;
    public float awarenessRadius = 8.0f;
    public float stoppingRadius = 6.0f;
    public Transform target;

    [Header("Ground Checks")]
    public float fallThreshold = 3.0f;
    public float groundCheckDistance = 0.3f;
    public Vector3 groundCheckOffset;
    public LayerMask environmentLayer;
    public float gravity = -9.81f;

    private Animator animator;

    [HideInInspector] public CharacterController characterController;

    private Statistics.Health health;

    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool isFalling;
    private bool isInteracting;
    [HideInInspector] public bool isRewinding;

    [HideInInspector] public float speedVertical;
    [HideInInspector] public Vector3 externalForces;
    [HideInInspector] public float fallDistance = 0.0f;
    [HideInInspector] public float fallInitialY = float.MinValue;

    private DataStructures.BehaviourSelector behaviourTree;

    private void Awake() {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        health = GetComponent<Statistics.Health>();
    }

    private void Start() {
        behaviourTree = EnemyTreeGenerator.GenerateBasicTree();
    }

    private void Update() {
        if (!isRewinding) {
            GroundCheck();
            FallCheck();
            UpdateTree();
            ResolveForces();
        }
    }

    private void LateUpdate() {
        isInteracting = animator.GetBool("IsInteracting");
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

            if (!isInteracting && fallDistance < 0.01f) {
                PlayTargetAnimation("Falling", true);
                fallDistance = hit.distance;
                if (fallInitialY <= float.MinValue + float.Epsilon) { fallInitialY = transform.position.y; }
            }

            if (hit.distance > fallDistance) { fallDistance = hit.distance; }
        }

        Vector3 velocity = new Vector3(0f, speedVertical, 0f);
        characterController.Move(velocity * Time.deltaTime);
    }

    private float CalculateFallDamage() {
        float threshold = fallThreshold * 1.5f;
        return fallDistance <= threshold || fallInitialY - transform.position.y <= threshold ? 0.0f : (fallDistance - threshold) * 8.25f;
    }

    private void UpdateTree() {
        behaviourTree.Reset();
        Debug.Log("=================================");
        DataStructures.BehaviourState state = DataStructures.BehaviourState.Ongoing;
        while (state == DataStructures.BehaviourState.Ongoing) {
            state = behaviourTree.Execute(Time.deltaTime, gameObject);
        }
    }

    private void PlayTargetAnimation(string targetAnimation, bool interactingState) {
        animator.SetBool("IsInteracting", interactingState);
        animator.CrossFade(targetAnimation, 0.2f);
    }

     public void AddForce(Vector3 direction, float magnitude) {
        if (!isRewinding) { externalForces += direction.normalized * magnitude; }
    }

    private void ResolveForces() {
        if (externalForces.magnitude > 0.2f) { characterController.Move(externalForces * Time.deltaTime); }
        externalForces = Vector3.Lerp(externalForces, Vector3.zero, 5 * Time.deltaTime);
    }
}
}