using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SudsGrow : MonoBehaviour {
    float growRate = 1;
    SoapMovement sm;
    Collider coll;
    GameObject bubbleTrigger;

    void Start () {
        coll = GetComponent<Collider>();
        sm = GameObject.FindWithTag("Player").GetComponent<SoapMovement>();
        bubbleTrigger = GameObject.Find("BubbleTrigger");
	}
	
	void Update () {
		if (transform.localScale.x < 1 && coll.enabled == true)
            transform.localScale += Vector3.one * growRate * Time.deltaTime;
        if (coll.enabled == false) {
            if (transform.localScale.x > 0)
                transform.localScale -= Vector3.one * growRate * Time.deltaTime;
            else
                Destroy(gameObject);
        }
	}
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.name == "BubbleTrigger" && other.gameObject.tag != "Colliders") {
            bubbleTrigger.SetActive(false);
            sm.canBoost = true;
            sm.canSuds = true;
        }
    }
}
