using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPunch : Ability {
    private GameObject punchPrefab;
    private GameObject punchInstance;

    protected override void Awake() {
        base.Awake();
        punchPrefab = Resources.Load<GameObject>("Abilities/Prefabs/GroundPunch");
    }

    public override void Activate() {
        //Spawno il pugno
        punchInstance = Instantiate(punchPrefab, transform.position, Quaternion.identity);
        punchInstance.GetComponentInChildren<DamageDealer>().Set(player.WhatIsEnemy(), data.damageAmount, data.damageType, data.hitSound);
    }

    public override void BeginCooldown() {
        Destroy(punchInstance);
    }
}
