using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Abilities {
[CreateAssetMenu(menuName = "Scriptable Objects/Ability")]      
public class Ability : ScriptableObject {
    public string abilityName;
    public AbilityType abilityType;
    public float energyCost;
    public float staminaCost;
    public float damage;
    public float range;
    public float cooldown;
    public float speed;
    public GameObject prefab;
}
}