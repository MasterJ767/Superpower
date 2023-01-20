using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers {
public class RecorderManager : MonoBehaviour {
    public float recordFrequency = 0.02f;
    public float recordDuration = 6.0f;

    private static RecorderManager instance;
    public static RecorderManager GetInstance() {
        if (instance == null) {
            instance = FindObjectOfType<RecorderManager>();
            if (instance == null) { Debug.Log("NO RECORDER MANAGER IN SCENE"); }
        }
        return instance;
    }

    private void Awake() {
        if (GetInstance() != this) {
            DestroyImmediate(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}
}