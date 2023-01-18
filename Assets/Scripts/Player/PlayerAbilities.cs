using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
public class PlayerAbilities : MonoBehaviour {
    private AttackMode mode = AttackMode.Basic;
    private PlayerMovement playerMovement;
    private Statistics.Information playerInfo;
    private Statistics.Stamina stamina;
    private Statistics.Energy energy;
    private Abilities.AbilityState basicAttack;
    private Abilities.AbilityState attack1;
    private Abilities.AbilityState attack2;
    private Abilities.AbilityState attack3;
    private Abilities.AbilityState attack4;
    private Abilities.AbilityState attack5;

    private void Awake() {
        playerMovement = GetComponent<PlayerMovement>();
        playerInfo = GetComponent<Statistics.Information>();
        stamina = GetComponent<Statistics.Stamina>();
        energy = GetComponent<Statistics.Energy>();

        basicAttack = new Abilities.AbilityState { canUse = true, cooldownTimer = 0};
        attack1 = new Abilities.AbilityState { canUse = true, cooldownTimer = 0};
        attack2 = new Abilities.AbilityState { canUse = true, cooldownTimer = 0};
        attack3 = new Abilities.AbilityState { canUse = true, cooldownTimer = 0};
        attack4 = new Abilities.AbilityState { canUse = true, cooldownTimer = 0};
        attack5 = new Abilities.AbilityState { canUse = true, cooldownTimer = 0};
    }

    private void Update() {
        UpdateCooldowns();
        AbilityCheck();
    }

    private void UpdateCooldowns() {
        if (!basicAttack.canUse) {
            basicAttack.cooldownTimer -= Time.deltaTime;
            basicAttack.canUse = basicAttack.cooldownTimer <= 0;
        }

        if (!attack1.canUse) {
            attack1.cooldownTimer -= Time.deltaTime;
            attack1.canUse = attack1.cooldownTimer <= 0;
        }

        if (!attack2.canUse) {
            attack2.cooldownTimer -= Time.deltaTime;
            attack2.canUse = attack2.cooldownTimer <= 0;
        }

        if (!attack3.canUse) {
            attack3.cooldownTimer -= Time.deltaTime;
            attack3.canUse = attack3.cooldownTimer <= 0;
        }

        if (!attack4.canUse) {
            attack4.cooldownTimer -= Time.deltaTime;
            attack4.canUse = attack4.cooldownTimer <= 0;
        }

        if (!attack5.canUse) {
            attack5.cooldownTimer -= Time.deltaTime;
            attack5.canUse = attack5.cooldownTimer <= 0;
        }
    }

    private void AbilityCheck() {
        Abilities.Ability ability;
        switch (mode) {
            case AttackMode.Basic:
                ability = playerInfo.baseAttack;
                if (Input.GetMouseButtonDown(0) && basicAttack.canUse && energy.ExpendQuery(ability.energyCost) > 0 && stamina.ExpendQuery(ability.staminaCost) > 0) {
                    energy.Expend(ability.energyCost);
                    stamina.Expend(ability.staminaCost);
                    //Handle basic attack
                }
                break;
        }
    }
}
}