using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Abilities {
public class BasicProjectile : MonoBehaviour {
    public float maxScale;
    public GameObject[] particleSystems;

    private Transform user;
    private Vector3 offset;
    private float damage;
    private float speed;
    private float range;
    private float inflateTime;
    private byte team;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    [HideInInspector] public bool isInitialised;
    [HideInInspector] public bool isInflated;
    [HideInInspector] public bool isRewinding;

    private void OnDrawGizmos() {
        if (isInitialised && !isInflated) {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, 0.5f * maxScale);
        }

        if (isInflated) {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(startPosition, targetPosition);
        }
    }

    private void Update() {
        if (!isRewinding && isInitialised && !isInflated) { 
            PositionCheck();
            InflateCheck(); 
        }
        if (!isRewinding && isInflated) { MoveProjectile(); }
        if (isInflated && Vector3.Distance(transform.position, targetPosition) <= float.Epsilon) { Decay(); }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Wall")) {

        }
        else if (collision.gameObject.CompareTag("Enemy") && team != 2) {

        }
        else if (collision.gameObject.CompareTag("Player") && team != 1) {

        }
        else if (collision.gameObject.CompareTag("Ally") && team != 1) {

        }
    }

    public void Initialise(Transform user, Vector3 offset, float damage, float speed, float range, float animationDelayTime, byte team) {
        this.user = user;
        this.offset = offset;
        this.damage = damage;
        this.speed = speed;
        this.range = range;
        this.inflateTime = animationDelayTime;
        this.team = team;
        isInitialised = true;       
    }

    private void PositionCheck() {
        transform.position = user.TransformPoint(offset);
    }

    private void InflateCheck() {
        float scale = transform.localScale.x + ((maxScale / inflateTime) * Time.deltaTime);
        scale = Mathf.Clamp(scale, 0, maxScale);
        transform.localScale = new Vector3(scale, scale, scale);
        foreach (GameObject particle in particleSystems) {
            particle.transform.localScale = new Vector3(scale, scale, scale);
        }
        if (scale + float.Epsilon >= maxScale && scale - float.Epsilon <= maxScale) { 
            isInflated = true; 
            if (user.CompareTag("Player")) {
                Transform camera = user.GetComponent<Player.PlayerMovement>().cameraController.transform;
                Vector3 lookDirection = new Vector3(user.forward.x, Mathf.Lerp(user.forward.y, camera.forward.y, 1.0f / 3.0f), user.forward.z);
                transform.rotation = Quaternion.LookRotation(lookDirection);
            }
            else {
                transform.rotation = Quaternion.identity;
            }
            startPosition = transform.position;
            targetPosition = startPosition + (transform.forward * range);        
        }
    }

    private void MoveProjectile() {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    public void Decay() {
        Destroy(gameObject);
    }
}
}