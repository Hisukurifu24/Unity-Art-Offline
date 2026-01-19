using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UraMawashiGeri : Ability {
    private GameObject punchPrefab;
    private AudioClip hitSound;
    private Vector2 pivot;
    private Vector2 mousePos;
    private int speed;

    protected override void Awake() {
        base.Awake();
        punchPrefab = Resources.Load<GameObject>("Abilities/Prefabs/UraMawashiGeri");
        hitSound = Resources.Load<AudioClip>("Sounds/Hit");
    }

    private void Start() {
        speed = 1000; //activeTime 0.15
    }

    public override void Activate() {
        //Spawno il pugno
        GameObject tsuki = Instantiate(punchPrefab, transform);
        tsuki.GetComponentInChildren<DamageDealer>().Set(player.WhatIsEnemy(), data.damageAmount, data.damageType, data.hitSound);

        GetComponent<SpriteRenderer>().enabled = false;

        //Calcolo la rotazione
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Vector2.SignedAngle(Vector2.right, mousePos - pivot);
        //transform.position = mousePos;

        //Posiziono il pugno
        tsuki.transform.RotateAround(pivot, Vector3.forward, angle);

        //Effettuo il pugno
        StartCoroutine(MoveAnimation(tsuki));
        StartCoroutine(RotateAnimation(tsuki));
    }

    private IEnumerator RotateAnimation(GameObject obj) {
        float time = data.activeTime;
        while (time > 0) {
            obj.transform.RotateAround(transform.position, Vector3.back, speed * Time.deltaTime);
            time -= Time.deltaTime;
            yield return null;
        }
        Destroy(obj);
        GetComponent<SpriteRenderer>().enabled = true;
    }
    private IEnumerator MoveAnimation(GameObject obj) {
        float time = data.activeTime;
        while (time > 0) {
            transform.position = Vector3.MoveTowards(transform.position, mousePos, 10f * Time.deltaTime);
                obj.transform.position = transform.position;
            time -= Time.deltaTime;
            yield return null;
        }
    }

    public override void BeginCooldown() { }

}
