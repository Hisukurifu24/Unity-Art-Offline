using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suriashi : Ability {
    float dashVelocity;
    Rigidbody2D rb;

    private void Start() {
        dashVelocity = 50;
        rb = GetComponent<Rigidbody2D>();
    }

    public override void Activate() {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        print(mousePos.normalized);

        rb.AddForce(mousePos.normalized * -dashVelocity, ForceMode2D.Impulse);
    }

    public override void BeginCooldown() { }
}
