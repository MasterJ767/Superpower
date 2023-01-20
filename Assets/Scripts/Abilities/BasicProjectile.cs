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

        Gizmos.color = Color.red;
        Gizmos.DrawLine(user.position, user.position + user.forward * 3);
        Transform camera = user.GetComponent<Player.PlayerMovement>().cameraController.transform;
        Gizmos.DrawLine(camera.position, camera.position + camera.forward * 3);

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

    private void Decay() {
        Destroy(gameObject);
    }
}
}