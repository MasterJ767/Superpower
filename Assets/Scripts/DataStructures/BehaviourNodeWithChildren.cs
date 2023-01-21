using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataStructures {
public class BehaviourNodeWithChildren : BehaviourNode {
    protected List<BehaviourNode> childNodes;

    public BehaviourNodeWithChildren(string nodeName) : base(nodeName) {}

    public void AddChild(BehaviourNode node) {
        childNodes.Add(node);
    }

    public override void Reset() {
        currentState = BehaviourState.Initialise;
        foreach (BehaviourNode n in childNodes) {
            n.Reset();
        }
    }
}
}