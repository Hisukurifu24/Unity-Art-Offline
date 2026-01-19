using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AbilityData : ScriptableObject {
    public Sprite icon;
    public string abilityName;
    public string description;

    public float cooldownTime;
    public float activeTime;

    [Header("Optional")]
    public float damageAmount;
    public Damage damageType;
    public AudioClip triggerSound;
    public AudioClip hitSound;
}
