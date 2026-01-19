using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public abstract class Ability : MonoBehaviour {
    public KeyCode key;
    
    protected AbilityData data;
    protected Player player;

    private float currentCooldownTime;
    private float currentActiveTime;
    private AbilityState state = AbilityState.ready;

    protected virtual void Awake() {
        data = Resources.Load<AbilityData>("Abilities/SO/" + GetType().ToString());
        if (data.hitSound != null) {
            data.hitSound = Resources.Load<AudioClip>("Sounds/Hit");
        }
        player = GetComponent<Player>();
    }

    private void Update() {
        switch (state) {
            case AbilityState.ready:
                foreach (Ability a in GetComponents<Ability>()) {
                    if (a.GetState() == AbilityState.active) {
                        return;
                    }
                }
                if (Input.GetKeyDown(key) && player.IsAvailable()) {
                    Activate();
                    state = AbilityState.active;
                    currentActiveTime = data.activeTime;
                }
                break;
            case AbilityState.active:
                if (currentActiveTime > 0) {
                    currentActiveTime -= Time.deltaTime;
                }
                else {
                    BeginCooldown();
                    state = AbilityState.cooldown;
                    currentCooldownTime = data.cooldownTime;
                }
                break;
            case AbilityState.cooldown:
                if (currentCooldownTime > 0) {
                    currentCooldownTime -= Time.deltaTime;
                }
                else {
                    state = AbilityState.ready;
                }
                break;
            default:
                break;
        }
    }

    public abstract void Activate();
    public abstract void BeginCooldown();

    public AbilityState GetState() {
        return state;
    }
}

public enum AbilityState {
    ready,
    active,
    cooldown
}
