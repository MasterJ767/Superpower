using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Abilities {
public class BasicProjectile : MonoBehaviour {
    public float maxScale;
    public float inflateTime;
    public GameObject[] particleSystems;

    private Transform user;
    private Vector3 offset;
    private float damage;
    private float speed;
    private float range;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool isInitialised;
    private bool isInflated;

    private void Update() {
        if (isInitialised && !isInflated) { 
            PositionCheck();
            InflateCheck(); 
        }
        if (isInflated) { MoveProjectile(); }
        if (isInflated && Vector3.Distance(transform.position, targetPosition) <= float.Epsilon) { Decay(); }
    }

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

    public void Initialise(Transform user, Vector3 offset, float damage, float speed, float range) {
        this.user = user;
        this.offset = offset;
        this.damage = damage;
        this.speed = speed;
        this.range = range;
        isInitialised = true;       
    }

    private void PositionCheck() {
        transform.position = user.TransformPoint(offset);
    }

    private void InflateCheck() {
        Debug.Log(transform.localScale.x);
        float scale = transform.localScale.x + ((maxScale / inflateTime) * Time.deltaTime);
        scale = Mathf.Clamp(scale, 0, maxScale);
        transform.localScale = new Vector3(scale, scale, scale);
        foreach (GameObject particle in particleSystems) {
            particle.transform.localScale = new Vector3(scale, scale, scale);
        }
        if (scale + float.Epsilon >= maxScale && scale - float.Epsilon <= maxScale) { 
            isInflated = true; 
            transform.rotation = user.CompareTag("Player") ? Quaternion.LookRotation(Vector3.Lerp(user.forward, user.GetComponent<Player.PlayerMovement>().cameraController.transform.forward, 1.0f / 3.0f)) : Quaternion.identity;
            startPosition = transform.position;
            targetPosition = startPosition + (transform.forward * range);        
        }
    }

    private void MoveProjectile() {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    private void Decay() {
        Destroy(gameObject);
    }
}
}