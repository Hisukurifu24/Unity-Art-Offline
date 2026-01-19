using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MawashiGeri : Ability {
    private GameObject punchPrefab;
    private AudioClip hitSound;
    private bool flip;
    private int speed;

    protected override void Awake() {
        base.Awake();
        punchPrefab = Resources.Load<GameObject>("Abilities/Prefabs/MawashiGeri");
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
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Vector2.SignedAngle(Vector2.right, mousePos);

        Debug.DrawLine(Vector3.zero, transform.position, Color.cyan, 5);

        //Se la rotazione è alle spalle del giocatore, flippo la texture
        tsuki.GetComponentInChildren<SpriteRenderer>().flipX = flip = (angle > 90 || angle < -90);
        angle = flip ? angle - 180 : angle;

        //Posiziono il pugno
        tsuki.transform.RotateAround(transform.position, Vector3.forward, angle);

        //Effettuo il pugno
        StartCoroutine(TsukiAnimation(tsuki));
    }

    private IEnumerator TsukiAnimation(GameObject obj) {
        float time = data.activeTime;
        while (time > 0) {
            obj.transform.RotateAround(transform.position, flip ? Vector3.forward : Vector3.back, speed * Time.deltaTime);
            time -= Time.deltaTime;
            yield return null;
        }
        Destroy(obj);
        GetComponent<SpriteRenderer>().enabled = true;
    }

    public override void BeginCooldown() { }
}
