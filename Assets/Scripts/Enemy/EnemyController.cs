using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy {
public class EnemyController : MonoBehaviour
{
    private DataStructures.BehaviourSelector behaviourTree;

    private void Start() {
        behaviourTree = new DataStructures.BehaviourSelector("parent");
    }

    private void Update() {
        UpdateTree();
    }

    private void UpdateTree() {
        behaviourTree.Reset();
        DataStructures.BehaviourState state = DataStructures.BehaviourState.Ongoing;
        while (state == DataStructures.BehaviourState.Ongoing) {
            state = behaviourTree.Execute(Time.deltaTime);
        }
    }
}
}