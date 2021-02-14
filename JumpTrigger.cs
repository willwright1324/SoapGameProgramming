using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTrigger : MonoBehaviour {
    public GameObject soap;
    GameObject empty;
    GameObject collidingWith;
    public SoapMovement sm;
    // Use this for initialization
    void Start () {
        soap = GameObject.FindWithTag("Player");
        empty = GameObject.Find("JumpEmpty");
        sm = soap.GetComponent<SoapMovement>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void LateUpdate() {
        empty.transform.eulerAngles = new Vector3(soap.transform.eulerAngles.x, soap.transform.eulerAngles.y, soap.transform.eulerAngles.z);
    }
    private void OnTriggerStay(Collider other) {
        if (other.gameObject.tag != "Water") {
            sm.canJump = true;
        }
    }
}
