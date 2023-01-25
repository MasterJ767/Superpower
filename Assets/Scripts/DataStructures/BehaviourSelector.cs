using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataStructures {
public class BehaviourSelector : BehaviourNodeWithChildren {
    public BehaviourSelector(string nodeName) : base(nodeName) {}

    public override BehaviourState Execute(float dt, GameObject self) {
        foreach (BehaviourNode node in childNodes) {
            BehaviourState nodeState = node.Execute(dt, self);
            switch(nodeState){
                case BehaviourState.Failure:
                    continue;
                case BehaviourState.Success:
                    currentState = nodeState;
                    return currentState;
                case BehaviourState.Ongoing:
                    currentState = nodeState;
                    return currentState;
            }
        }
        return BehaviourState.Failure;
    }
}
}