using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Statistics {
[CreateAssetMenu(menuName = "Scriptable Objects/KineticClass")]    
public class KineticClass : ScriptableObject {
    [Header("Attributes")]
    public KineticType kineticType;
    public Color primary;
    public Color secondary;
    public Abilities.Ability[] abilities;
}
}