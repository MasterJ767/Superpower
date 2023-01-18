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
        ModeCheck();
        AbilityCheck();
    }

    private void UpdateCooldowns() {
        if (!basicAttack.canUse) {
            basicAttack.cooldownTimer -= Time.deltaTime;
            Debug.Log(basicAttack.cooldownTimer);
            basicAttack.canUse = basicAttack.cooldownTimer <= 0;
            Debug.Log(basicAttack.canUse);
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

    private void ModeCheck() {
        if (Input.GetButtonDown("Basic")) { mode = AttackMode.Basic; }
        else if (Input.GetButtonDown("Ability1")) { mode = AttackMode.Attack1; }
        else if (Input.GetButtonDown("Ability2")) { mode = AttackMode.Attack2; }
        else if (Input.GetButtonDown("Ability3")) { mode = AttackMode.Attack3; }
        else if (Input.GetButtonDown("Ability4")) { mode = AttackMode.Attack4; }
        else if (Input.GetButtonDown("Ability5")) { mode = AttackMode.Attack5; }
    }

    private void AbilityCheck() {
        Abilities.Ability ability;
        switch (mode) {
            case AttackMode.Basic:
                ability = playerInfo.baseAttack;
                if (Input.GetMouseButton(0) && basicAttack.canUse && energy.ExpendQuery(ability.energyCost) > 0 && stamina.ExpendQuery(ability.staminaCost) > 0) {
                    basicAttack.canUse = false;
                    basicAttack.cooldownTimer = ability.cooldown;
                    energy.Expend(ability.energyCost);
                    stamina.Expend(ability.staminaCost);
                    playerMovement.PlayAttackAnimation("BasicAttack");
                    //Handle basic attack
                }
                break;
            case AttackMode.Attack1:
                ability = playerInfo.attack1;
                if (Input.GetMouseButton(0) && attack1.canUse && energy.ExpendQuery(ability.energyCost) > 0 && stamina.ExpendQuery(ability.staminaCost) > 0) {
                    attack1.canUse = false;
                    attack1.cooldownTimer = ability.cooldown;
                    energy.Expend(ability.energyCost);
                    stamina.Expend(ability.staminaCost);
                    //Handle attack
                }
                break;
            case AttackMode.Attack2:
                ability = playerInfo.attack2;
                if (Input.GetMouseButton(0) && attack2.canUse && energy.ExpendQuery(ability.energyCost) > 0 && stamina.ExpendQuery(ability.staminaCost) > 0) {
                    attack2.canUse = false;
                    attack2.cooldownTimer = ability.cooldown;
                    energy.Expend(ability.energyCost);
                    stamina.Expend(ability.staminaCost);
                    //Handle attack
                }
                break;
            case AttackMode.Attack3:
                ability = playerInfo.attack3;
                if (Input.GetMouseButton(0) && attack3.canUse && energy.ExpendQuery(ability.energyCost) > 0 && stamina.ExpendQuery(ability.staminaCost) > 0) {
                    attack3.canUse = false;
                    attack3.cooldownTimer = ability.cooldown;
                    energy.Expend(ability.energyCost);
                    stamina.Expend(ability.staminaCost);
                    //Handle attack
                }
                break;
            case AttackMode.Attack4:
                ability = playerInfo.attack4;
                if (Input.GetMouseButton(0) && attack4.canUse && energy.ExpendQuery(ability.energyCost) > 0 && stamina.ExpendQuery(ability.staminaCost) > 0) {
                    attack4.canUse = false;
                    attack4.cooldownTimer = ability.cooldown;
                    energy.Expend(ability.energyCost);
                    stamina.Expend(ability.staminaCost);
                    //Handle attack
                }
                break;
            case AttackMode.Attack5:
                ability = playerInfo.attack5;
                if (Input.GetMouseButton(0) && attack5.canUse && energy.ExpendQuery(ability.energyCost) > 0 && stamina.ExpendQuery(ability.staminaCost) > 0) {
                    attack5.canUse = false;
                    attack5.cooldownTimer = ability.cooldown;
                    energy.Expend(ability.energyCost);
                    stamina.Expend(ability.staminaCost);
                    //Handle attack
                }
                break;
        }
    }
}
}