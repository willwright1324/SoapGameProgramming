using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouseMovement : MonoBehaviour {
    GameObject soap, empty, anchor;
    int xOffset = 0;
    int yOffset = 5;
    int zOffset = -15;
    int zoomSpeed = 50;
    int clampRange = 65;
    public float rY, rX;
    public bool playerBlocked;
    public string coll;
    Vector3 lookDir;
    Vector3 hitPoint;
    float maxDist;
    StartCamera sCam;

    void Start() {
        soap   = GameObject.FindWithTag("Player");
        empty  = GameObject.Find("CameraEmpty");
        anchor = GameObject.Find("CameraAnchor");
        sCam   = GameObject.Find("StartCamera").GetComponent<StartCamera>();

        transform.position        = new Vector3(xOffset, yOffset, zOffset);
        anchor.transform.position = new Vector3(xOffset, yOffset, zOffset);
        maxDist = Vector3.Distance(empty.transform.position, anchor.transform.position);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update() {
        float mX = 0;
        float mY = 0;
        if (sCam.done == true) {
            mX = Input.GetAxis("Mouse X");
            mY = -Input.GetAxis("Mouse Y");
        }
        rX += mY;
        float rX2 = rX;
        rY += mX;
        rX = Mathf.Clamp(rX, -60, clampRange);
        rX2 = Mathf.Clamp(rX2, -20, clampRange);
        empty.transform.rotation = Quaternion.Euler(rX2, rY, 0);
        if (rX < -20 && rX > -60)
            transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x + mY, 0, 0);
        else {
            if (rX > -20)
                transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        lookDir = (anchor.transform.position - empty.transform.position).normalized;
        CastRay();
    }
    private void LateUpdate() {
        empty.transform.position = soap.transform.position;
        if (playerBlocked)
            transform.position = Vector3.MoveTowards(transform.position, hitPoint, Time.deltaTime * zoomSpeed);
        else
            transform.position = Vector3.MoveTowards(transform.position, anchor.transform.position, Time.deltaTime * zoomSpeed);
    }

    void CastRay() {
        if (Physics.Raycast(soap.transform.position, lookDir, out RaycastHit hit, maxDist)) {
            coll = hit.collider.gameObject.name;
            if (!(hit.collider.gameObject.tag == "Player"
               || hit.collider.gameObject.name == "JumpTrigger"
               || hit.collider.gameObject.tag == "Suds"
               || hit.collider.gameObject.name == "Water"
               || hit.collider.gameObject.tag == "Enemy")) {
                hitPoint = hit.point;
                playerBlocked = true;
            }
        }
        else
            playerBlocked = false;
    }
}