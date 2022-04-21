using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofFade : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collision) {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.1f);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
    }
}
