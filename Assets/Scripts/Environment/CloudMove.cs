using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMove : MonoBehaviour
{
    public float speed;

    private void Update() {
        transform.Translate(Vector3.left * Time.deltaTime * speed);
    }
}
