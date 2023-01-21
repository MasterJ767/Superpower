using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Statistics {
public class Information : MonoBehaviour {
    [Header("General")]
    public string entityName;
    public KineticType entityClass;
    public int level;
    
    [Header("Attacks")]
    public Abilities.Ability baseAttack;
    public Abilities.Ability attack1;
    public Abilities.Ability attack2;
    public Abilities.Ability attack3;
    public Abilities.Ability attack4;
    public Abilities.Ability attack5;
}
}