using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DamageDealer : MonoBehaviour
{
    private string whatIsEnemy;
    private float damageAmount;
    private Damage damageType;
    private AudioClip hitSound;

    public void Set(string wie, float da, Damage dt, AudioClip hs) {
        whatIsEnemy = wie;
        damageAmount = da;
        damageType = dt;
        hitSound = hs;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag(whatIsEnemy) && !collision.isTrigger) {
            GetComponent<AudioSource>().clip = hitSound;
            GetComponent<AudioSource>().Play();
            collision.GetComponent<Player>().TakeDamage(damageAmount, damageType);
        }
    }
}
