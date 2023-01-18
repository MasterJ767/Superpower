using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers {
public class KineticManager : MonoBehaviour {
    public Statistics.KineticClass[] classList;

    private static KineticManager instance;
    public static KineticManager GetInstance() {
        if (instance == null) {
            instance = FindObjectOfType<KineticManager>();
            if (instance == null) { Debug.Log("NO KINETIC MANAGER IN SCENE"); }
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

    public Statistics.KineticClass GetClass(Statistics.KineticType search) {
        foreach (Statistics.KineticClass kineticClass in classList) {
            if (kineticClass.kineticType == search) { 
                return kineticClass;
            }
        }
        return null;
    }
}
}