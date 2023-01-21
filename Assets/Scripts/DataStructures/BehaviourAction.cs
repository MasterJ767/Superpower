using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataStructures {
public class BehaviourAction : BehaviourNode
{
    private Func<float, BehaviourState, BehaviourState> function;

    public BehaviourAction(string nodeName, Func<float, BehaviourState, BehaviourState> function) : base(nodeName) {
        this.function = function;
    }

    public override BehaviourState Execute(float dt) {
        currentState = function(dt, currentState);
        return currentState;
    }
}
}