using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleBehavior : MonoBehaviour {
    public float size;
    float growRate = 0.5f;
    bool flip;
	// Use this for initialization
	void Start () {
        size = Random.Range(0.4f, 1.5f);
        if (size > 0.725) {
            flip = true;
        }
    }

    // Update is called once per frame
    void Update () {
        
        if (flip) {
            if (size < 1) {
                size += growRate * Time.deltaTime;
            }
            else {
                flip = false;
            }
        }
        else {
            if (size > 0.4) {
                size -= growRate * Time.deltaTime;
            }
            else {
                flip = true;
            }
        }
        transform.localScale = Vector3.one * size;
        
    }
}
