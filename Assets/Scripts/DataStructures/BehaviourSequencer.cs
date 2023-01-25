using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataStructures {
public class BehaviourSequencer : BehaviourNodeWithChildren {
    public BehaviourSequencer(string nodeName) : base(nodeName) {}

    public override BehaviourState Execute(float dt, GameObject self) {
        foreach (BehaviourNode node in childNodes) {
            BehaviourState nodeState = node.Execute(dt, self);
            switch(nodeState){
                case BehaviourState.Success:
                    continue;
                case BehaviourState.Failure:
                    currentState = nodeState;
                    return currentState;
                case BehaviourState.Ongoing:
                    currentState = nodeState;
                    return currentState;
            }
        }
        return BehaviourState.Success;
    }
}
}