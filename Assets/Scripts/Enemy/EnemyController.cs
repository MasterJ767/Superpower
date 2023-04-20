using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy {
public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 500.0f;
    public float visionRadius = 25.0f;
    public float awarenessRadius = 8.0f;
    public float stoppingRadius = 6.0f;
    public Transform target;
    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public bool isRewinding;

    private DataStructures.BehaviourSelector behaviourTree;

    private void Awake() {
        characterController = GetComponent<CharacterController>();
    }

    private void Start() {
        behaviourTree = EnemyTreeGenerator.GenerateBasicTree();
    }

    private void Update() {
        UpdateTree();
    }

    private void UpdateTree() {
        behaviourTree.Reset();
        DataStructures.BehaviourState state = DataStructures.BehaviourState.Ongoing;
        while (state == DataStructures.BehaviourState.Ongoing) {
            state = behaviourTree.Execute(Time.deltaTime, gameObject);
        }
    }
}
}