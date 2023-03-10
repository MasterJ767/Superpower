using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Abilities {
[CreateAssetMenu(menuName = "Scriptable Objects/Ability")]      
public class Ability : ScriptableObject {
    public string abilityName;
    public AbilityType abilityType;
    public AbilityEffect abilityEffect;
    public string animationName;
    public float animationDelayTime;
    public float energyCost;
    public float staminaCost;
    public float damage;
    public float range;
    public float cooldown;
    public float speed;
    public float duration;
    public GameObject prefab;
    public Vector3 prefabOffset;
}
}