using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopEyesRotation : MonoBehaviour {
    GameObject soap, pivot, cam;
    public float direc, timer;
    // Use this for initialization
    void Start() {
        soap = GameObject.FindWithTag("Player");
        pivot = GameObject.Find("EyesPivot");
        cam = GameObject.Find("CameraEmpty");
    }
    void FaceDirection() {
        if (!Input.anyKey) {
            if (timer > 0) {
                timer -= Time.deltaTime;
            }
            else {
                pivot.transform.rotation = Quaternion.Lerp(pivot.transform.rotation, Quaternion.Euler(-cam.transform.eulerAngles.x, cam.transform.eulerAngles.y + 180, 0), Time.deltaTime * 5);
            }
        }
        else {
            timer = 1.5f;
        }
        if (Input.GetKey("a")) {
            pivot.transform.rotation = Quaternion.Lerp(pivot.transform.rotation, Quaternion.Euler(0, cam.transform.eulerAngles.y - 90, 0), Time.deltaTime * 15);
        }
        if (Input.GetKey("d")) {
            pivot.transform.rotation = Quaternion.Lerp(pivot.transform.rotation, Quaternion.Euler(0, cam.transform.eulerAngles.y + 90, 0), Time.deltaTime * 15);
        }
        if (Input.GetKey("w")) {
            pivot.transform.rotation = Quaternion.Lerp(pivot.transform.rotation, Quaternion.Euler(0, cam.transform.eulerAngles.y, 0), Time.deltaTime * 15);
        }
        if (Input.GetKey("s")) {
            pivot.transform.rotation = Quaternion.Lerp(pivot.transform.rotation, Quaternion.Euler(0, cam.transform.eulerAngles.y + 180, 0), Time.deltaTime * 15);
        }
    }
    // Update is called once per frame
    void Update() {
        direc = Vector3.Dot(soap.transform.up, Vector3.down);
        if (direc > 0.1f) {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(179, 0, 0), Time.deltaTime * 10);
        }
        if (direc < -0.1f) {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(1, 0, 0), Time.deltaTime * 10);
        }
        if (!Input.GetKey(KeyCode.Mouse0) && (direc > 0.8f || direc < -0.8f)) {
            FaceDirection();
        }
    }
}
