using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeTrigger : MonoBehaviour {
    GameObject soap, eyes, empty, camEmpty;
    public float direc;
    public float xOffset, yOffset, zOffset, xOffset2, yOffset2, zOffset2;

    void Start() {
        soap = GameObject.FindWithTag("Player");
        camEmpty = GameObject.Find("CameraEmpty");
        eyes = GameObject.Find("SoapEyes");
        empty = GameObject.Find("EyesEmpty");
        xOffset = xOffset2 = eyes.transform.localPosition.x;
        yOffset = yOffset2 = eyes.transform.localPosition.y;
        zOffset = zOffset2 = eyes.transform.localPosition.z;
    }

    void Update() {
        direc = Vector3.Dot(soap.transform.forward, camEmpty.transform.right);
        eyes.transform.localPosition = Vector3.MoveTowards(eyes.transform.localPosition, new Vector3(xOffset, yOffset, zOffset + Mathf.Abs(direc)), Time.deltaTime * 25);
        transform.localPosition = new Vector3(0, 0.4f, zOffset2 + Mathf.Abs(direc));
        empty.transform.rotation = Quaternion.Euler(0, camEmpty.transform.eulerAngles.y, 0);

    }
    private void OnTriggerStay(Collider other) {
        if (other.gameObject.name != "SoapSide" && other.gameObject.name != "SlipperySide" && other.gameObject.name != "JumpTrigger" && other.gameObject.name != "Bubble(Clone)" && other.gameObject.name != "Water" && other.gameObject.name != "Points" && other.gameObject.name != "BigPoints") {
            xOffset = 0;
            yOffset = -0.2f;
            zOffset = 0;
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.name != "SoapSide" && other.gameObject.name != "SlipperySide" && other.gameObject.name != "JumpTrigger" && other.gameObject.name != "Bubble(Clone)" && other.gameObject.name != "Water" && other.gameObject.name != "Points" && other.gameObject.name != "BigPoints") {
            xOffset = xOffset2;
            yOffset = yOffset2;
            zOffset = zOffset2;
        }
    }
}
