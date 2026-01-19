using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Stealth : Ability {
    protected override void Awake() {
        base.Awake();
    }

    public override void Activate() {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
    }

    public override void BeginCooldown() {
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
