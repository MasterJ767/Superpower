using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recorder {
public class PlayerRecorder : MonoBehaviour, IRecorder {
    [Header("UI")]
    public GameObject timeVignette;

    private Managers.RecorderManager recorderManager;
    private Player.PlayerMovement playerMovement;
    private Player.PlayerAbilities playerAbilities;
    private Player.CameraController cameraController;
    private Statistics.Health playerHealth;
    private Statistics.Stamina playerStamina;
    private Statistics.Energy playerEnergy;

    private List<PlayerState> history;
    private bool isRecording = true;

    private void Awake() {
        recorderManager = Managers.RecorderManager.GetInstance();
        playerMovement = gameObject.GetComponent<Player.PlayerMovement>();
        playerAbilities = gameObject.GetComponent<Player.PlayerAbilities>();
        cameraController = playerMovement.cameraController;
        playerHealth = gameObject.GetComponent<Statistics.Health>();
        playerStamina = gameObject.GetComponent<Statistics.Stamina>();
        playerEnergy = gameObject.GetComponent<Statistics.Energy>();
        history = new List<PlayerState>();
    }

    private void Start() {
        StartCoroutine(Record());
    }

    public IEnumerator Record() {
        while (isRecording) {
            PlayerState state = new PlayerState() {
                position = transform.position,
                rotation = transform.rotation,
                cameraPosition = cameraController.transform.position,
                cameraRotation = cameraController.transform.rotation,
                isGrounded = playerMovement.isGrounded,
                isFalling = playerMovement.isFalling,
                speedHorizontal = playerMovement.speedHorizontal,
                speedVertical = playerMovement.speedVertical,
                externalForces = playerMovement.externalForces,
                fallDistance = playerMovement.fallDistance,
                fallInitialY = playerMovement.fallInitialY,
                attackMode = playerAbilities.mode,
                basicAttackCooldown = playerAbilities.basicAttack.cooldownTimer,
                attack1Cooldown = playerAbilities.attack1.cooldownTimer,
                attack2Cooldown = playerAbilities.attack2.cooldownTimer,
                attack3Cooldown = playerAbilities.attack3.cooldownTimer,
                attack4Cooldown = playerAbilities.attack4.cooldownTimer,
                attack5Cooldown = playerAbilities.attack5.cooldownTimer,
                currentHealth = playerHealth.currentHealth,
                timeSinceHit = playerHealth.timeSinceHit,
                currentStamina = playerStamina.currentStamina,
                timeSinceStaminaUse = playerStamina.timeSinceUse,
                timeSinceEnergyUse = playerEnergy.timeSinceUse
            };
            history.Add(state);

            while(history.Count > ((1.0f / recorderManager.recordFrequency) * recorderManager.recordDuration)) {
                history.RemoveAt(0);
            }

            yield return new WaitForSeconds(recorderManager.recordFrequency);
        }
    }

    public IEnumerator Rewind(bool self, float time) 
    {
        ToggleRewind(true);

        int states = Mathf.FloorToInt((1.0f / recorderManager.recordFrequency) * time);
        int historyStates = history.Count;
        Stack<PlayerState> historyStack = states >= historyStates ? new Stack<PlayerState>(history) : new Stack<PlayerState>(history.GetRange(historyStates - states, states));

        while (historyStack.Count > 0) {
            PlayerState state = historyStack.Pop();
            transform.position = state.position;
            transform.rotation = state.rotation;
            cameraController.transform.position = state.cameraPosition;
            cameraController.transform.rotation = state.cameraRotation;
            playerMovement.isGrounded = state.isGrounded;
            playerMovement.isFalling = state.isFalling;
            playerMovement.speedHorizontal = state.speedHorizontal;
            playerMovement.speedVertical = state.speedVertical;
            playerMovement.externalForces = state.externalForces;
            playerMovement.fallDistance = state.fallDistance;
            playerMovement.fallInitialY = state.fallInitialY;
            playerAbilities.mode = state.attackMode;
            playerAbilities.UpdateBorder();
            playerAbilities.basicAttack.cooldownTimer = state.basicAttackCooldown;
            playerAbilities.attack1.cooldownTimer = state.attack1Cooldown;
            playerAbilities.attack2.cooldownTimer = state.attack2Cooldown;
            playerAbilities.attack3.cooldownTimer = state.attack3Cooldown;
            playerAbilities.attack4.cooldownTimer = state.attack4Cooldown;
            playerAbilities.attack5.cooldownTimer = state.attack5Cooldown;
            playerHealth.currentHealth = self ? Mathf.Max(state.currentHealth, playerHealth.currentHealth) : Mathf.Min(state.currentHealth, playerHealth.currentHealth);
            playerHealth.timeSinceHit = state.timeSinceHit;
            playerHealth.SetHealthSlider();
            playerStamina.currentStamina = self ? Mathf.Max(state.currentStamina, playerStamina.currentStamina) : Mathf.Min(state.currentStamina, playerStamina.currentStamina);
            playerStamina.timeSinceUse = state.timeSinceStaminaUse;
            playerStamina.SetStaminaSlider();
            playerEnergy.timeSinceUse = state.timeSinceEnergyUse;
            yield return new WaitForSeconds(recorderManager.recordFrequency);
        }

        if (historyStates < states) { yield return new WaitForSeconds((states - historyStates) * recorderManager.recordFrequency); }

        ToggleRewind(false);
    }

    public void ToggleRewind(bool value) {
        timeVignette.SetActive(value);
        playerMovement.isRewinding = value;
        cameraController.isRewinding = value;
        playerAbilities.isRewinding = value;
        playerHealth.isRewinding = value;
        playerStamina.isRewinding = value;
        playerEnergy.isRewinding = value;
    }

    public void StartRewind(float time) {
        StartCoroutine(Rewind(true, time));
    }

    public void StartSetback(float time) {
        StartCoroutine(Rewind(false, time));
    }
}
}