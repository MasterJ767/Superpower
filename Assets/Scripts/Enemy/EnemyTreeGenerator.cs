using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataStructures;

namespace Enemy {
public static class EnemyTreeGenerator {
    public static BehaviourSelector GenerateBasicTree() {

        BehaviourAction verifyTarget = new BehaviourAction("Verify Target", EnemyTreeFunctions.VerifyTarget);
        BehaviourAction moveTowardTarget = new BehaviourAction("Move Toward Target", EnemyTreeFunctions.MoveTowardTarget);

        BehaviourSequencer continueRoute = new BehaviourSequencer("Continue Route");
        continueRoute.AddChild(verifyTarget);
        continueRoute.AddChild(moveTowardTarget);

        BehaviourAction findTarget = new BehaviourAction("Find Target", EnemyTreeFunctions.FindTarget);
        BehaviourAction attackTarget = new BehaviourAction("Attack Target", EnemyTreeFunctions.AttackTarget);

        BehaviourSequencer sequenceAttack = new BehaviourSequencer("Sequence Attack");
        sequenceAttack.AddChild(findTarget);
        sequenceAttack.AddChild(attackTarget);

        BehaviourAction idle = new BehaviourAction("Idle", EnemyTreeFunctions.Idle);

        BehaviourSelector root = new BehaviourSelector("Root");
        root.AddChild(verifyTarget);
        root.AddChild(sequenceAttack);
        root.AddChild(idle);
        
        return root;
    }

    
}
}