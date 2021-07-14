using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedWaterBehavior : MonoBehaviour {
    bool surfacing;
    bool swimming;
    public bool sink;
    bool shake;
    public float shakeIntensity = 0.4f;
    public float shakeTime = 50;
    public float floatTime = 2;
    public float recoverTime = 5;
    public float surfaceDecay = 1.5f;
    float surfaceTimer;
    public float floatForce = 100;
    public float depth;
    public float velClamp = 5;
    Rigidbody rb;
    GameObject water;

    void Start() {
        water = GameObject.Find("Water");
        rb    = GetComponent<Rigidbody>();
    }

    void Update() {
        depth = (water.transform.position.y + (water.transform.localScale.y / 2)) - transform.position.y;
        if (shake) {
            if (!sink) {
                if (shakeTime > 25) {
                    transform.Rotate(shakeIntensity, 0, 0);
                    shakeTime--;
                }
                else {
                    if (shakeTime > 0) {
                        transform.Rotate(-shakeIntensity, 0, 0);
                        shakeTime--;
                    }
                    else
                        shakeTime = 50;
                }
            }
            if (floatTime > 0) floatTime -= Time.deltaTime;
            else               sink = true;
        }
        else {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(Vector3.zero), Time.deltaTime);
            if (floatTime < 3)
                floatTime += Time.deltaTime;
        }
        if (sink && !shake) {
            if (recoverTime > 0) recoverTime -= Time.deltaTime;
            else                 sink = false;
        }
    }
    private void FixedUpdate() {
        //Applies force based on depth
        if (swimming && !sink) {
            rb.GetComponent<ConstantForce>().force = Vector3.up * depth * floatForce;
            rb.velocity = new Vector3(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -velClamp, velClamp), rb.velocity.z);
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y / 1.1f, rb.velocity.z);
        }
        else {
            rb.useGravity = true;
            rb.GetComponent<ConstantForce>().force = Vector3.zero;
        }

        //Decays velocity when coming out of water
        if (surfacing && surfaceTimer < 5) {
            rb.velocity /= surfaceDecay;
            surfaceTimer++;
        }
        else {
            surfacing = false;
            surfaceTimer = 0;
        }
    }
    private void OnCollisionStay(Collision collision) {
        if (collision.gameObject.tag == "Ground")
            gameObject.tag = "Ground";
        if (collision.gameObject.tag == "Player") {
            shake = true;
            recoverTime = 5;
        }
    }
    private void OnCollisionExit(Collision collision) {
        if (collision.gameObject.tag == "Ground")
            gameObject.tag = "Untagged";
        if (collision.gameObject.tag == "Player")
            shake = false;
    }
    private void OnTriggerStay(Collider other) {
        surfacing = false;
        if (other.gameObject.tag == "Water") {
            if (!sink)
                rb.useGravity = false;
            swimming = true;
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Water") {
            rb.useGravity = true;
            swimming = false;
            surfacing = true;
        }
    }
}
