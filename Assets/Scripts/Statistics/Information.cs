using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Statistics {
public class Information : MonoBehaviour {
    [Header("General")]
    public string entityName;
    public KineticType entityClass;
    public int level;
    public SkinnedMeshRenderer meshRenderer;
    
    [Header("Attacks")]
    public Abilities.Ability baseAttack;
    public Abilities.Ability attack1;
    public Abilities.Ability attack2;
    public Abilities.Ability attack3;
    public Abilities.Ability attack4;
    public Abilities.Ability attack5;

    public void SetFresnel(bool state) {
        foreach(Material material in meshRenderer.materials) {
            material.SetInt("_Fresnel", state ? 1 : 0);
        }
    }

    public void SetFresnelColour(bool state, Color colour) {
        foreach(Material material in meshRenderer.materials) {
            material.SetColor("_FresnelColour", colour);
            material.SetInt("_Fresnel", state ? 1 : 0);
        }
    }
}
}