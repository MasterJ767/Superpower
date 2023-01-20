using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recorder {
public struct PlayerState {
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 cameraPosition;
    public Quaternion cameraRotation;
    public float speedHorizontal;
    public float speedVertical;
    public Vector3 externalForces;
    public float fallDistance;
    public float fallInitialY;
    public Player.AttackMode attackMode;
    public float basicAttackCooldown;
    public float attack1Cooldown;
    public float attack2Cooldown;
    public float attack3Cooldown;
    public float attack4Cooldown;
    public float attack5Cooldown;
    public float currentHealth;
    public float timeSinceHit;
    public float currentStamina;
    public float timeSinceStaminaExpend;
    public float currentEnergy;
    public float timeSinceEnergyExpend;
}
}