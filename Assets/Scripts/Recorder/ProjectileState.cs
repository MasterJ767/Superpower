using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recorder {
public struct ProjectileState {
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
    public bool isInitialised;
    public bool isInflated;
}
}