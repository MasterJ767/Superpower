using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recorder {
public class ProjectileRecorder : MonoBehaviour {
    private Managers.RecorderManager recorderManager;

    private List<ProjectileState> history;

    private void Awake() {
        recorderManager = Managers.RecorderManager.GetInstance();
    }

    private void Start() {
        StartCoroutine(Record());
    }

    public IEnumerator Record() {
        ProjectileState state = new ProjectileState() {
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
        List<ProjectileState> historyCopy = states >= historyStates ? new List<ProjectileState>(history) : new List<ProjectileState>(history.GetRange(historyStates - states, states));

        while (historyCopy.Count > 0) {
            // transfer state info
            yield return new WaitForSeconds(recorderManager.recordFrequency);
        }

        // Enable everything
    }
}
}