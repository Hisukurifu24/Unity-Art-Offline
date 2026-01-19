using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyDash : Ability {
    private GameObject punchPrefab;
    private Vector2 mousePos;

    protected override void Awake() {
        base.Awake();
        punchPrefab = Resources.Load<GameObject>("Abilities/Prefabs/DeadlyDash");
    }

    private void Start() {
    }

    public override void Activate() {
        //Spawno il pugno
        GameObject tsuki = Instantiate(punchPrefab, transform);
        tsuki.GetComponentInChildren<DamageDealer>().Set(player.WhatIsEnemy(), data.damageAmount, data.damageType, data.hitSound);

        //Calcolo la rotazione
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        //Effettuo il pugno
        StartCoroutine(MoveAnimation(tsuki));
    }

    private IEnumerator MoveAnimation(GameObject obj) {
        float time = data.activeTime;
        while (time > 0) {
            GetComponent<Rigidbody2D>().MovePosition(mousePos);

            //transform.position = Vector3.MoveTowards(transform.position, mousePos, speed * Time.deltaTime);
            //obj.transform.position = transform.position;
            time -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        Destroy(obj);
    }

    public override void BeginCooldown() { }
}
