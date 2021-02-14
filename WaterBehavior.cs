using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBehavior : MonoBehaviour {
    bool surfacing;
    bool swimming;
    public float surfaceDecay = 1.5f;
    float surfaceTimer;
    public float floatForce = 100;
    public float depth;
    public float velClamp = 5;
    Rigidbody rb;
    GameObject water;
    // Use this for initialization
    void Start () {
        water = GameObject.Find("Water");
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        depth = (water.transform.position.y + (water.transform.localScale.y / 2)) - transform.position.y;
    }
    private void FixedUpdate() {
        //Applies force based on depth
        if (swimming) {
            rb.GetComponent<ConstantForce>().force = Vector3.up * depth * floatForce;
            rb.velocity = new Vector3(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -velClamp, velClamp), rb.velocity.z);
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y / 1.1f, rb.velocity.z);
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
    private void OnTriggerStay(Collider other) {
        surfacing = false;
        if (other.gameObject.tag == "Water") {
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
