using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recorder {
public class PlayerRecorder : MonoBehaviour, IRecorder {
    private Managers.RecorderManager recorderManager;
    private Player.PlayerMovement playerMovement;
    private Player.PlayerAbilities playerAbilities;
    private Player.CameraController cameraController;
    private Statistics.Health playerHealth;
    private Statistics.Stamina playerStamina;
    private Statistics.Energy playerEnergy;

    private List<PlayerState> history;

    private void Awake() {
        recorderManager = Managers.RecorderManager.GetInstance();
        playerMovement = gameObject.GetComponent<Player.PlayerMovement>();
        playerAbilities = gameObject.GetComponent<Player.PlayerAbilities>();
        cameraController = playerMovement.cameraController;
        playerHealth = gameObject.GetComponent<Statistics.Health>();
        playerStamina = gameObject.GetComponent<Statistics.Stamina>();
        playerEnergy = gameObject.GetComponent<Statistics.Energy>();
    }

    private void Start() {
        StartCoroutine(Record());
    }

    public IEnumerator Record() {
        PlayerState state = new PlayerState() {
            // add state info here
        };
        history.Add(state);

        while(history.Count > ((1.0f / recorderManager.recordFrequency) * recorderManager.recordDuration)) {
            history.RemoveAt(0);
        }

        yield return new WaitForSeconds(recorderManager.recordFrequency);
    }

    public IEnumerator Rewind(bool self, float time) 
    {
        // Disable everything

        int states = Mathf.FloorToInt((1.0f / recorderManager.recordFrequency) * time);
        int historyStates = history.Count;
        List<PlayerState> historyCopy = states >= historyStates ? new List<PlayerState>(history) : new List<PlayerState>(history.GetRange(historyStates - states, states));

        while (historyCopy.Count > 0) {
            // transfer state info
            yield return new WaitForSeconds(recorderManager.recordFrequency);
        }

        // Enable everything
    }
}
}