using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Abilities {
public static class AbilityCaster {
   public static void ExecuteBasicAttack(Transform user, Ability baseAttack, AbilityState attackModifiers, byte team) {
      GameObject projectile = GameObject.Instantiate(baseAttack.prefab, user.position + baseAttack.prefabOffset, user.rotation);
      projectile.GetComponent<BasicProjectile>().Initialise(user, baseAttack.prefabOffset, baseAttack.damage, baseAttack.speed, baseAttack.range, baseAttack.animationDelayTime, team);
   }

   public static void ExecuteMeleeAttack() {

   }

   public static void ExecuteBeamAttack() {

   }

   public static void ExecuteSelf(Transform user, Ability baseAttack, AbilityState attackModifiers) {
      switch (baseAttack.abilityEffect) {
         case AbilityEffect.Rewind:
            user.GetComponent<Recorder.IRecorder>().StartRewind(baseAttack.duration);
            break;
      }
   }
}
}