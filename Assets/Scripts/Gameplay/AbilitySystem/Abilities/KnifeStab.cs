using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeStab : Ability {
    private GameObject punchPrefab;
    private AudioClip hitSound;
    private Vector2 pivot;
    private int speed;

    protected override void Awake() {
        base.Awake();
        punchPrefab = Resources.Load<GameObject>("Abilities/Prefabs/KnifeStab");
        hitSound = Resources.Load<AudioClip>("Sounds/Hit");
    }

    private void Start() {
        speed = 50;
    }

    public override void Activate() {
        //Spawno il pugno
        GameObject tsuki = Instantiate(punchPrefab, transform);
        tsuki.GetComponentInChildren<DamageDealer>().Set(player.WhatIsEnemy(), data.damageAmount, data.damageType, data.hitSound);

        //Calcolo pivot del pugno (+offset)
        pivot = (Vector2)transform.position + new Vector2(0, 0.25f);

        //Calcolo la rotazione
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Vector2.SignedAngle(Vector2.right, mousePos - pivot);

        //Se la rotazione è alle spalle del giocatore, flippo la texture
        tsuki.GetComponentInChildren<SpriteRenderer>().flipX = (angle > 90 || angle < -90) ? true : false;

        //Posiziono il pugno
        tsuki.transform.RotateAround(pivot, Vector3.forward, angle);

        //Effettuo il pugno
        StartCoroutine(TsukiAnimation(tsuki));
    }

    private IEnumerator TsukiAnimation(GameObject obj) {
        float time = data.activeTime;
        while (time > data.activeTime / 2) {
            obj.transform.Translate(1 * speed * Time.deltaTime, 0, 0);
            time -= Time.deltaTime;
            yield return null;
        }
        while (time > 0) {
            obj.transform.Translate(1 * -speed * Time.deltaTime, 0, 0);
            time -= Time.deltaTime;
            yield return null;
        }
        Destroy(obj);
    }

    public override void BeginCooldown() { }

}
