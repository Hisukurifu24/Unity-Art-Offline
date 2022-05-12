using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Dash : Ability
{
    public float dashVelocity;

    public override void Activate(GameObject parent) {
        PlayerController p = parent.GetComponent<PlayerController>();
        Rigidbody2D rb = parent.GetComponent<Rigidbody2D>();

        Debug.Log("BLINK");
        Debug.Log(p.moveCommand * dashVelocity);
        rb.AddForce(p.moveCommand * dashVelocity, ForceMode2D.Impulse);
    }
}
