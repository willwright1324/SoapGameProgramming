using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GermPathMovement : MonoBehaviour {
    public float movementSpeed = 3;
    public float squishSpeed = 10;
    public float pauseTime = 2;
    public bool loop;
    Transform path;
    Transform[] paths, forwPaths, backPaths;
    Vector3 tempPos;
    int index = 1;
    int tempIndex;
    float timer;
    bool moving, pause, swap;

	void Start () {
        Transform[] children = GetComponentsInChildren<Transform>();
        List<Transform> tempPath = new List<Transform>();
        List<Transform> backTemp = new List<Transform>();
        for (int i = 0; i < children.Length; i++) {
            if (children[i].tag == "Path")
                tempPath.Add(children[i]);
            if (children[i].name == "Path")
                path = children[i];
        }
        for (int i = children.Length - 1; i > 0; i--) {
            if (children[i].tag == "Path")
                backTemp.Add(children[i]);
        }
        path.parent = null;
        forwPaths = tempPath.ToArray();
        backPaths = backTemp.ToArray();
        paths = forwPaths;
        transform.position = paths[0].position;
        transform.eulerAngles = paths[0].eulerAngles;
	}
	
	void Update () {
		if (!moving) {
            if (index < paths.Length)
                moving = true;
            else {
                if (!loop) {
                    if (swap) {
                        paths = forwPaths;
                        swap = false;
                    }
                    else {
                        paths = backPaths;
                        swap = true;
                    }
                    index = 1;
                }
                else
                    index = 0;
            }
        }
        else {
            if (index == 0) tempIndex = paths.Length - 1;
            else            tempIndex = index - 1;

            if (transform.localScale.y > 0.21f && !pause) {
                tempPos = new Vector3(paths[tempIndex].position.x, paths[tempIndex].position.y - 1.6f, paths[tempIndex].position.z);
                transform.position = Vector3.Lerp(transform.position, tempPos, movementSpeed * 2 * Time.deltaTime);
                transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1.04f, 0.2f, 1.04f), squishSpeed * Time.deltaTime);
            }
            else {
                tempPos = new Vector3(paths[index].position.x, paths[index].position.y - 1.6f, paths[index].position.z);
                if (Vector3.Distance(transform.position, tempPos) > 0.1f && !pause) {
                    transform.position = Vector3.Lerp(transform.position, tempPos, movementSpeed * Time.deltaTime);
                    transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, paths[index].eulerAngles, movementSpeed * Time.deltaTime);
                }
                else {
                    pause = true;
                    if (timer < pauseTime) {
                        transform.position = Vector3.Lerp(transform.position, paths[index].position, movementSpeed * 2* Time.deltaTime);
                        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, squishSpeed * Time.deltaTime);
                        timer += Time.deltaTime;
                    }
                    else {
                        timer = 0;
                        index++;
                        pause = false;
                        moving = false;
                    }
                }
            }
        }
	}
}
