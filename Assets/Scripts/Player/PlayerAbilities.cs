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

    private void Awake() {
        playerMovement = GetComponent<PlayerMovement>();
        playerInfo = GetComponent<Statistics.Information>();
        stamina = GetComponent<Statistics.Stamina>();
        energy = GetComponent<Statistics.Energy>();

        basicAttack = new Abilities.AbilityState { cooldownTimer = 0};
        attack1 = new Abilities.AbilityState { cooldownTimer = 0};
        attack2 = new Abilities.AbilityState { cooldownTimer = 0};
        attack3 = new Abilities.AbilityState { cooldownTimer = 0};
        attack4 = new Abilities.AbilityState { cooldownTimer = 0};
        attack5 = new Abilities.AbilityState { cooldownTimer = 0};
    }

    private void Update() {
        if (!isRewinding) {
            UpdateCooldowns();
            ModeCheck();
            AbilityCheck();
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

    private void ModeCheck() {
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

    private void AbilityCheck() {
        Abilities.Ability ability;
        switch (mode) {
            case AttackMode.Basic:
                ability = playerInfo.baseAttack;
                if (Input.GetMouseButton(0) && basicAttack.cooldownTimer <= 0 && energy.ExpendQuery(ability.energyCost) > 0 && stamina.ExpendQuery(ability.staminaCost) > 0) {
                    basicAttack.cooldownTimer = HandleAbility(ability, basicAttack);
                }
                break;
            case AttackMode.Attack1:
                ability = playerInfo.attack1;
                if (Input.GetMouseButton(0) && attack1.cooldownTimer <= 0 && energy.ExpendQuery(ability.energyCost) > 0 && stamina.ExpendQuery(ability.staminaCost) > 0) {
                    attack1.cooldownTimer = HandleAbility(ability, attack1);
                }
                break;
            case AttackMode.Attack2:
                ability = playerInfo.attack2;
                if (Input.GetMouseButton(0) && attack2.cooldownTimer <= 0 && energy.ExpendQuery(ability.energyCost) > 0 && stamina.ExpendQuery(ability.staminaCost) > 0) {
                    HandleAbility(ability, attack2);
                }
                break;
            case AttackMode.Attack3:
                ability = playerInfo.attack3;
                if (Input.GetMouseButton(0) && attack3.cooldownTimer <= 0 && energy.ExpendQuery(ability.energyCost) > 0 && stamina.ExpendQuery(ability.staminaCost) > 0) {
                    HandleAbility(ability, attack3);
                }
                break;
            case AttackMode.Attack4:
                ability = playerInfo.attack4;
                if (Input.GetMouseButton(0) && attack4.cooldownTimer <= 0 && energy.ExpendQuery(ability.energyCost) > 0 && stamina.ExpendQuery(ability.staminaCost) > 0) {
                    HandleAbility(ability, attack4);
                }
                break;
            case AttackMode.Attack5:
                ability = playerInfo.attack5;
                if (Input.GetMouseButton(0) && attack5.cooldownTimer <= 0 && energy.ExpendQuery(ability.energyCost) > 0 && stamina.ExpendQuery(ability.staminaCost) > 0) {
                    HandleAbility(ability, attack5);
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
                Abilities.GenericAbilities.ExecuteBasicAttack(transform, ability, abilityProperties, team);
                break;
            case Abilities.AbilityType.Self:
                Abilities.GenericAbilities.ExecuteSelf(transform, ability, abilityProperties);
                break;
        }

        return abilityProperties.cooldownTimer;
    }
}
}