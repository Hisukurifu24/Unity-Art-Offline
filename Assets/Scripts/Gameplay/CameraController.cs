using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform follow;

    private void Start() {
        follow = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update() {
        if (Input.mouseScrollDelta.y > 0 && Camera.main.orthographicSize > 1) {
            Camera.main.orthographicSize -= 0.5f;
        }
        else if (Input.mouseScrollDelta.y < 0 && Camera.main.orthographicSize < 5) {
            Camera.main.orthographicSize += 0.5f;
        }

        transform.position = new Vector3(follow.position.x, follow.position.y, -10);
    }
}
