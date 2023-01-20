using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Abilities {
public static class GenericAbilities {
   public static void ExecuteBasicAttack(Transform user, Vector3 instantiationOffset, Ability baseAttack, AbilityState attackModifiers) {
      GameObject projectile = GameObject.Instantiate(baseAttack.prefab, user.position + instantiationOffset, Quaternion.identity);
      projectile.transform.localScale = Vector3.zero;
      projectile.GetComponent<BasicProjectile>().Initialise(user, instantiationOffset, baseAttack.damage, baseAttack.speed, baseAttack.range);
   }

   public static void ExecuteMeleeAttack() {

   }

   public static void ExecuteBeamAttack() {

   }
}
}