using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Latching : MonoBehaviour {
    GameObject soap;
    SoapMovement sm;
	// Use this for initialization
	void Start () {
        soap = GameObject.FindWithTag("Player");
        sm = soap.GetComponent<SoapMovement>();
    }

    // Update is called once per frame
    void Update () {
		
	}
    private void OnTriggerStay(Collider other) {
        if (other.gameObject.tag == "Latchable") {
            sm.canLatch = true;
        }
    }
    private void OnTriggerExit(Collider other) {
        sm.canLatch = false;
    }
}
