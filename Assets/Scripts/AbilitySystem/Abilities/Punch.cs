using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : Ability {

    private GameObject punchPrefab;
    private AudioClip hitSound;
    private Vector2 pivot;

    private bool leftPunch;
    private float swingSpeed;
    private float duration;

    private void Start() {
        punchPrefab = Resources.Load<GameObject>("Abilities/Prefabs/Punch");
        hitSound = Resources.Load<AudioClip>("Sounds/Hit");
        leftPunch = false;
        swingSpeed = 1000;
        duration = data.activeTime;
    }

    public override void Activate() {
        pivot = (Vector2)transform.position + new Vector2(0, 0.25f);

        GameObject p = Instantiate(punchPrefab, transform);

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Vector2.SignedAngle(Vector2.up, mousePos - pivot);


        Debug.DrawLine(transform.position, pivot, Color.magenta, 3);
        Debug.DrawLine(transform.position, mousePos, Color.yellow, 3);

        p.transform.RotateAround(pivot, Vector3.forward, leftPunch ? angle - 45 : angle + 45);

        StartCoroutine(SwingAnimation(p));
        leftPunch = !leftPunch;
    }

    public override void BeginCooldown() {    }

    private IEnumerator SwingAnimation(GameObject obj) {
        float time = duration;
        while (time > 0) {
            obj.transform.RotateAround(pivot, leftPunch ? Vector3.back : Vector3.forward, swingSpeed * Time.deltaTime);
            time -= Time.deltaTime;
            yield return null;
        }
        Destroy(obj);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Enemy") && !collision.isTrigger) {
            GetComponent<AudioSource>().clip = hitSound;
            GetComponent<AudioSource>().Play();
            Destroy(collision.gameObject);
        }
    }
}
