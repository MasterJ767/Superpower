using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recorder {
public class ProjectileRecorder : MonoBehaviour, IRecorder {
    private Managers.RecorderManager recorderManager;
    private Abilities.BasicProjectile projectileController;

    private List<ProjectileState> history;
    private bool isRecording = true;

    private void Awake() {
        recorderManager = Managers.RecorderManager.GetInstance();
        projectileController = gameObject.GetComponent<Abilities.BasicProjectile>();
        history = new List<ProjectileState>();
    }

    private void Start() {
        StartCoroutine(Record());
    }

    public IEnumerator Record() {
        while (isRecording) {
            ProjectileState state = new ProjectileState() {
                position = transform.position,
                rotation = transform.rotation,
                scale = transform.localScale,
                isInitialised = projectileController.isInitialised,
                isInflated = projectileController.isInflated
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
        Stack<ProjectileState> historyStack = states >= historyStates ? new Stack<ProjectileState>(history) : new Stack<ProjectileState>(history.GetRange(historyStates - states, states));

        while (historyStack.Count > 0) {
            ProjectileState state = historyStack.Pop();
            transform.position = state.position;
            transform.rotation = state.rotation;
            transform.localScale = state.scale;
            projectileController.isInitialised = state.isInitialised;
            projectileController.isInflated = state.isInflated;
            yield return new WaitForSeconds(recorderManager.recordFrequency);
        }

        if (!projectileController.isInitialised || historyStates < states) { projectileController.Decay(); }

        ToggleRewind(false);
    }

    public void ToggleRewind(bool value) {
        projectileController.isRewinding = value;
    }

    public void StartRewind(float time) {
        StartCoroutine(Rewind(true, time));
    }

    public void StartSetback(float time) {
        StartCoroutine(Rewind(false, time));
    }
}
}