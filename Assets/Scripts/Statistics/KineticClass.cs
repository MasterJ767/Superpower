using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Statistics {
[CreateAssetMenu(menuName = "Scriptable Objects/KineticClass")]    
public class KineticClass : ScriptableObject {
    [Header("Attributes")]
    public KineticType kineticType;
    public Abilities.Ability[] abilities;
}
}