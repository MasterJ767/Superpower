using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataStructures {
public class BehaviourAction : BehaviourNode
{
    private Func<float, GameObject, BehaviourState, BehaviourState> function;

    public BehaviourAction(string nodeName, Func<float, GameObject, BehaviourState, BehaviourState> function) : base(nodeName) {
        this.function = function;
    }

    public override BehaviourState Execute(float dt, GameObject self) {
        currentState = function(dt, self, currentState);
        return currentState;
    }
}
}