using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideEyesRotation : MonoBehaviour {
    GameObject soap;
    public float direc;
    // Use this for initialization
    void Start() {
        soap = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update() {
        direc = Vector3.Dot(soap.transform.up, Vector3.down);

        if (direc > 0.5) {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, 0, 179), Time.deltaTime * 10);
        }
        else {
            if (direc < -0.5) {
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, 0, 1), Time.deltaTime * 10);
            }
        }
    }
}
