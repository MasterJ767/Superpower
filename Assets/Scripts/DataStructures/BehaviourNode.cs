using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataStructures {
public class BehaviourNode {
    public string nodeName;
    protected BehaviourState currentState;

    public BehaviourNode(string nodeName) {
        this.nodeName = nodeName;
        currentState = BehaviourState.Initialise;
    }

    public virtual BehaviourState Execute(float dt) {
        return BehaviourState.Success;
    }

    public BehaviourState GetState() {
        return currentState;
    }

    public virtual void Reset() {
        currentState = BehaviourState.Initialise;
    }
}
}