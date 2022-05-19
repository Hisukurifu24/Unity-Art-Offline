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
}
