using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
public class PlayerAbilities : MonoBehaviour {
    [Header("UI")]
    public GameObject classBorder;
    public GameObject ability1Border;
    public GameObject ability2Border;
    public GameObject ability3Border;
    public GameObject ability4Border;
    public GameObject ability5Border;
    public LayerMask enemyLayer;

    [HideInInspector] public AttackMode mode = AttackMode.Basic;
    private PlayerMovement playerMovement;
    private Statistics.Information playerInfo;
    private Statistics.Stamina stamina;
    private Statistics.Energy energy;
    private readonly byte team = 1;
    [HideInInspector] public Abilities.AbilityState basicAttack;
    [HideInInspector] public Abilities.AbilityState attack1;
    [HideInInspector] public Abilities.AbilityState attack2;
    [HideInInspector] public Abilities.AbilityState attack3;
    [HideInInspector] public Abilities.AbilityState attack4;
    [HideInInspector] public Abilities.AbilityState attack5;
    [HideInInspector] public bool isRewinding;
    private Transform target = null;

    private void Awake() {
        playerMovement = GetComponent<PlayerMovement>();
        playerInfo = GetComponent<Statistics.Information>();
        stamina = GetComponent<Statistics.Stamina>();
        energy = GetComponent<Statistics.Energy>();

        basicAttack = new Abilities.AbilityState { cooldownTimer = 0 };
        attack1 = new Abilities.AbilityState { cooldownTimer = 0 };
        attack2 = new Abilities.AbilityState { cooldownTimer = 0 };
        attack3 = new Abilities.AbilityState { cooldownTimer = 0 };
        attack4 = new Abilities.AbilityState { cooldownTimer = 0 };
        attack5 = new Abilities.AbilityState { cooldownTimer = 0 };
    }

    private void OnDrawGizmos() {
        /*if ((mode == AttackMode.Basic && playerInfo.attack1?.abilityType == Abilities.AbilityType.Target) ||
            (mode == AttackMode.Attack1 && playerInfo.attack2?.abilityType == Abilities.AbilityType.Target) ||
            (mode == AttackMode.Attack2 && playerInfo.attack3?.abilityType == Abilities.AbilityType.Target) ||
            (mode == AttackMode.Attack3 && playerInfo.attack4?.abilityType == Abilities.AbilityType.Target) ||
            (mode == AttackMode.Attack4 && playerInfo.attack5?.abilityType == Abilities.AbilityType.Target)) {
                //Gizmos.DrawLine(transform.position + new Vector3(0, 1.35f, 0));
        }*/
    }

    private void Update() {
        if (!isRewinding) {
            UpdateCooldowns();
            Abilities.Ability ability = ModeCheck();
            TargetCheck(ability);
            AbilityCheck(ability);
        }
    }

    private void UpdateCooldowns() {
        basicAttack.cooldownTimer = basicAttack.cooldownTimer > 0 ? basicAttack.cooldownTimer - Time.deltaTime : 0;
        attack1.cooldownTimer = attack1.cooldownTimer > 0 ? attack1.cooldownTimer - Time.deltaTime : 0;
        attack2.cooldownTimer = attack2.cooldownTimer > 0 ? attack2.cooldownTimer - Time.deltaTime : 0;
        attack3.cooldownTimer = attack3.cooldownTimer > 0 ? attack3.cooldownTimer - Time.deltaTime : 0;
        attack4.cooldownTimer = attack4.cooldownTimer > 0 ? attack4.cooldownTimer - Time.deltaTime : 0;
        attack5.cooldownTimer = attack5.cooldownTimer > 0 ? attack5.cooldownTimer - Time.deltaTime : 0;
    }

    private Abilities.Ability ModeCheck() {
        AttackMode lastMode = mode;
        if (Input.GetButtonDown("Basic")) { mode = AttackMode.Basic; }
        else if (Input.GetButtonDown("Ability1")) { mode = mode == AttackMode.Attack1 ? AttackMode.Basic : AttackMode.Attack1; }
        else if (Input.GetButtonDown("Ability2")) { mode = mode == AttackMode.Attack2 ? AttackMode.Basic : AttackMode.Attack2; }
        else if (Input.GetButtonDown("Ability3")) { mode = mode == AttackMode.Attack3 ? AttackMode.Basic : AttackMode.Attack3; }
        else if (Input.GetButtonDown("Ability4")) { mode = mode == AttackMode.Attack4 ? AttackMode.Basic : AttackMode.Attack4; }
        else if (Input.GetButtonDown("Ability5")) { mode = mode == AttackMode.Attack5 ? AttackMode.Basic : AttackMode.Attack5; }
        if (lastMode != mode) {
            switch (lastMode) {
                case AttackMode.Basic: 
                    classBorder.SetActive(false);
                    break;
                case AttackMode.Attack1: 
                    ability1Border.SetActive(false);
                    break;
                case AttackMode.Attack2: 
                    ability2Border.SetActive(false);
                    break;
                case AttackMode.Attack3: 
                    ability3Border.SetActive(false);
                    break;
                case AttackMode.Attack4: 
                    ability4Border.SetActive(false);
                    break;
                case AttackMode.Attack5: 
                    ability5Border.SetActive(false);
                    break;
            }
            switch (mode) {
                case AttackMode.Basic: 
                    classBorder.SetActive(true);
                    break;
                case AttackMode.Attack1: 
                    ability1Border.SetActive(true);
                    break;
                case AttackMode.Attack2: 
                    ability2Border.SetActive(true);
                    break;
                case AttackMode.Attack3: 
                    ability3Border.SetActive(true);
                    break;
                case AttackMode.Attack4: 
                    ability4Border.SetActive(true);
                    break;
                case AttackMode.Attack5: 
                    ability5Border.SetActive(true);
                    break;
            }
        }

        switch (mode) {
            case AttackMode.Basic: 
                return playerInfo.baseAttack;
            case AttackMode.Attack1: 
                return playerInfo.attack1;
            case AttackMode.Attack2: 
                return playerInfo.attack2;
            case AttackMode.Attack3: 
                return playerInfo.attack3;
            case AttackMode.Attack4: 
                return playerInfo.attack4;
            case AttackMode.Attack5: 
                return playerInfo.attack5;
            default:
                return playerInfo.baseAttack;
        }
    }

    public void UpdateBorder() {
        classBorder.SetActive(false);
        ability1Border.SetActive(false);
        ability2Border.SetActive(false);
        ability3Border.SetActive(false);
        ability4Border.SetActive(false);
        ability5Border.SetActive(false);

        switch (mode) {
            case AttackMode.Basic: 
                classBorder.SetActive(true);
                break;
            case AttackMode.Attack1: 
                ability1Border.SetActive(true);
                break;
            case AttackMode.Attack2: 
                ability2Border.SetActive(true);
                break;
            case AttackMode.Attack3: 
                ability3Border.SetActive(true);
                break;
            case AttackMode.Attack4: 
                ability4Border.SetActive(true);
                break;
            case AttackMode.Attack5: 
                ability5Border.SetActive(true);
                break;
        }
    }

    private void TargetCheck(Abilities.Ability ability) {
        if (ability.abilityType != Abilities.AbilityType.Target) { return; }

        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0, 1.35f, 0), transform.GetComponent<Player.PlayerMovement>().cameraController.transform.forward, out hit, ability.range, enemyLayer)) {
            Enemy.EnemyController enemy = hit.collider.GetComponent<Enemy.EnemyController>();
            if (!enemy.isRewinding) {
                target = enemy.transform;
            }
            else {
                target = null;
            }
        }
        else {
            target = null;
        }
    }

    private void AbilityCheck(Abilities.Ability ability) {
        if (ability.abilityType == Abilities.AbilityType.Target && target == null) { return; }

        switch (mode) {
            case AttackMode.Basic:
                if (Input.GetMouseButton(0) && basicAttack.cooldownTimer <= 0 && energy.ExpendQuery(ability.energyCost) > 0 && stamina.ExpendQuery(ability.staminaCost) > 0) {
                    basicAttack.cooldownTimer = HandleAbility(ability, basicAttack);
                }
                break;
            case AttackMode.Attack1:
                if (Input.GetMouseButton(0) && attack1.cooldownTimer <= 0 && energy.ExpendQuery(ability.energyCost) > 0 && stamina.ExpendQuery(ability.staminaCost) > 0) {
                    attack1.cooldownTimer = HandleAbility(ability, attack1);
                }
                break;
            case AttackMode.Attack2:
                if (Input.GetMouseButton(0) && attack2.cooldownTimer <= 0 && energy.ExpendQuery(ability.energyCost) > 0 && stamina.ExpendQuery(ability.staminaCost) > 0) {
                    attack2.cooldownTimer = HandleAbility(ability, attack2);
                }
                break;
            case AttackMode.Attack3:
                if (Input.GetMouseButton(0) && attack3.cooldownTimer <= 0 && energy.ExpendQuery(ability.energyCost) > 0 && stamina.ExpendQuery(ability.staminaCost) > 0) {
                    attack3.cooldownTimer = HandleAbility(ability, attack3);
                }
                break;
            case AttackMode.Attack4:
                if (Input.GetMouseButton(0) && attack4.cooldownTimer <= 0 && energy.ExpendQuery(ability.energyCost) > 0 && stamina.ExpendQuery(ability.staminaCost) > 0) {
                    attack4.cooldownTimer = HandleAbility(ability, attack4);
                }
                break;
            case AttackMode.Attack5:
                if (Input.GetMouseButton(0) && attack5.cooldownTimer <= 0 && energy.ExpendQuery(ability.energyCost) > 0 && stamina.ExpendQuery(ability.staminaCost) > 0) {
                    attack5.cooldownTimer = HandleAbility(ability, attack5);
                }
                break;
        }

    }

    private float HandleAbility(Abilities.Ability ability, Abilities.AbilityState abilityProperties) {
        abilityProperties.cooldownTimer = ability.cooldown;
        energy.Expend(ability.energyCost);
        stamina.Expend(ability.staminaCost);
        if (!string.IsNullOrEmpty(ability.animationName)) { playerMovement.PlayAttackAnimation(ability.animationName); }

        switch (ability.abilityType) {
            case Abilities.AbilityType.Basic:
                Abilities.AbilityCaster.ExecuteBasicAttack(transform, ability, abilityProperties, team);
                break;
            case Abilities.AbilityType.Self:
                Abilities.AbilityCaster.ExecuteSelf(transform, ability, abilityProperties);
                break;
            case Abilities.AbilityType.Target:
                Abilities.AbilityCaster.ExecuteTarget(target, ability, abilityProperties);
                target = null;
                break;

        }

        return abilityProperties.cooldownTimer;
    }
}
}