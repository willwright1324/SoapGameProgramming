using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GermMovement : MonoBehaviour {
    public bool turn = true;
    public int squishSpeed = 10;
    public float timer;
    public int waitTime = 3;
    public float travelLength = 6;
    public float speed = 3;
    float pos1, pos2;
    BoxCollider sCollider, mCollider;
    Vector3 normal, squished;

	// Use this for initialization
	void Start () {
        pos1 = transform.position.x - travelLength;
        pos2 = transform.position.x;
        sCollider = GetComponent<BoxCollider>();
        mCollider = GetComponent<BoxCollider>();
        normal = new Vector3(1.0f, 3.820457f, 2.461333f);
        squished = new Vector3(1.0f, 1.093628f, 4.616637f);

        sCollider.size = normal;
    }
	
	// Update is called once per frame
	void Update () {
        if (transform.position.x >= pos1 && turn) {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1.04f, 0.2f, 1.04f), speed * squishSpeed * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, new Vector3(pos1 - 0.5f, transform.position.y, transform.position.z), speed * Time.deltaTime);
            sCollider.size = normal;
            timer = 0;
        }
        else {
            if (timer < waitTime && turn) {
                transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0.6f, 0.6f, 0.6f), speed * squishSpeed * Time.deltaTime);
                timer += Time.deltaTime;
                mCollider.size = squished;
            }
            else {
                turn = false;
            }
        }
        if (transform.position.x <= pos2 && !turn) {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1.04f, 0.2f, 1.04f), speed * squishSpeed * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, new Vector3(pos2 + 0.5f, transform.position.y, transform.position.z), speed * Time.deltaTime);
            sCollider.size = normal;
            timer = 0;
        }
        else {
            if (timer < waitTime && !turn) {
                transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0.6f, 0.6f, 0.6f), speed * squishSpeed * Time.deltaTime);
                timer += Time.deltaTime;
                mCollider.size = squished; 
            }
            else {
                turn = true;
            }
        }
    }
}
