using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBoomerang : Ability {
    private GameObject punchPrefab;
    private AudioClip hitSound;
    private Vector2 pivot;
    private Vector2 mousePos;
    private int rotateSpeed;
    private int moveSpeed;

    protected override void Awake() {
        base.Awake();
        punchPrefab = Resources.Load<GameObject>("Abilities/Prefabs/SwordBoomerang");
        hitSound = Resources.Load<AudioClip>("Sounds/Hit");
    }

    private void Start() {
        rotateSpeed = 1250; //activeTime 0.15
        moveSpeed = 10;
    }

    public override void Activate() {
        //Spawno il pugno
        GameObject tsuki = Instantiate(punchPrefab, transform);
        tsuki.GetComponentInChildren<DamageDealer>().Set(player.WhatIsEnemy(), data.damageAmount, data.damageType, data.hitSound);

        //Calcolo la rotazione
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Effettuo il pugno
        StartCoroutine(MoveAnimation(tsuki));
        StartCoroutine(RotateAnimation(tsuki));
    }

    private IEnumerator RotateAnimation(GameObject obj) {
        float time = data.activeTime;
        while (time > 0) {
            obj.transform.RotateAround(obj.transform.position, Vector3.back, rotateSpeed * Time.deltaTime);
            time -= Time.deltaTime;
            yield return null;
        }
        Destroy(obj);
    }
    private IEnumerator MoveAnimation(GameObject obj) {
        float time = data.activeTime;
        while (time > data.activeTime / 2) {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, mousePos, moveSpeed * Time.deltaTime);
            time -= Time.deltaTime;
            yield return null;
        }
        while (time > 0) {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, transform.position, moveSpeed * Time.deltaTime);
            time -= Time.deltaTime;
            yield return null;
        }
    }

    public override void BeginCooldown() { }

}
