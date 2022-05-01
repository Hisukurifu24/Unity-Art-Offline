using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Name", menuName = "SpellInfo", order = 1)]
public class SpellInfo : ScriptableObject
{
    public Sprite icon;
    public string spellName = "NONE";
    public string description = "NONE";
    public float cost = 0;
    public float cooldown = 0;
    public Dodgeability dodgeability;
    public GameObject prefab;
}

public enum Dodgeability {
    None = 0,
    Low = 5,
    Medium = 10,
    High = 25,
}
