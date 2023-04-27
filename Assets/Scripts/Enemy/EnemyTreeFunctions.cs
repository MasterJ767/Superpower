using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataStructures;

namespace Enemy {
public static class EnemyTreeFunctions
{
    public static BehaviourState VerifyTarget(float dt, GameObject self, BehaviourState state) {
        if (state == BehaviourState.Initialise) { return BehaviourState.Ongoing; }
        return self.GetComponent<EnemyController>().target == null ? BehaviourState.Failure : BehaviourState.Success;
    }

    public static BehaviourState MoveTowardTarget(float dt, GameObject self, BehaviourState state) {
        if (state == BehaviourState.Initialise) { return BehaviourState.Ongoing; }
        EnemyController enemyController = self.GetComponent<EnemyController>();
        Vector3 targetPosition = enemyController.target.position;
        targetPosition.y = 0;
        Vector3 enemyPosition = self.transform.position;
        enemyPosition.y = 0;
        Vector3 moveDirection = (targetPosition - enemyPosition).normalized;
        Vector3 velocity = moveDirection * enemyController.moveSpeed;
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        enemyController.characterController.Move(velocity * Time.deltaTime);
        self.transform.rotation = Quaternion.RotateTowards(self.transform.rotation, targetRotation, enemyController.rotationSpeed * Time.deltaTime);
        return Vector3.Distance(enemyPosition, targetPosition) <= enemyController.stoppingRadius ? BehaviourState.Failure : BehaviourState.Success;
    }

    public static BehaviourState FindTarget(float dt, GameObject self, BehaviourState state) {
        if (state == BehaviourState.Initialise) { return BehaviourState.Ongoing; }
        EnemyController enemyController = self.GetComponent<EnemyController>();
        if (enemyController.target != null) { return BehaviourState.Success; }
        Collider[] colliders = Physics.OverlapSphere(enemyController.transform.position, enemyController.visionRadius);
        if (colliders.Length == 0) { return BehaviourState.Failure; }
        List<Transform> canBeSeen = new List<Transform>();
        List<Transform> isAwareOf = new List<Transform>();
        foreach (Collider collider in colliders) {
            if (collider.CompareTag("Player") || collider.CompareTag("Ally")) {
                Vector3 direction = collider.transform.position - self.transform.position;
                if (direction.magnitude <= enemyController.awarenessRadius) { 
                    isAwareOf.Add(collider.transform); 
                    continue;
                }
                float dot = Vector3.Dot(direction.normalized, self.transform.forward);
                if (dot >= 0.5f) {
                    canBeSeen.Add(collider.transform);
                    continue;
                }
            }
        }
        if (canBeSeen.Count == 0 && isAwareOf.Count == 0) { return BehaviourState.Failure; }
        List<Transform> options = isAwareOf.Count == 0 ? canBeSeen : isAwareOf;
        Transform target = null;
        float maxDistance = float.MaxValue;
        foreach (Transform option in options) {
            float distance = Vector3.Distance(option.position, self.transform.position);
            if (distance < maxDistance){
                maxDistance = distance;
                target = option;
            }
        }
        enemyController.target = target;
        return BehaviourState.Success;
    }

    public static BehaviourState AttackTarget(float dt, GameObject self, BehaviourState state) {
        if (state == BehaviourState.Initialise) { return BehaviourState.Ongoing; }
        EnemyController enemyController = self.GetComponent<EnemyController>();
        Vector3 targetPosition = enemyController.target.position;
        targetPosition.y = 0;
        Vector3 enemyPosition = self.transform.position;
        enemyPosition.y = 0;
        if (Vector3.Distance(enemyPosition, targetPosition) > enemyController.stoppingRadius) { return BehaviourState.Failure; }
        // Add the ability for the enemy to attack here
        return BehaviourState.Success;
    }

    public static BehaviourState Idle(float dt, GameObject self, BehaviourState state) {
        if (state == BehaviourState.Initialise) { return BehaviourState.Ongoing; }
        // Add idle action here
        EnemyController enemyController = self.GetComponent<EnemyController>();
        enemyController.characterController.Move(self.transform.forward * Time.deltaTime);
        return BehaviourState.Success;
    }
}
}