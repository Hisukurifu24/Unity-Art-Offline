using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class ProgressBar : MonoBehaviour {
    private Slider slider;
    public float currentValue = 0;
    public float targetValue;

    private void Awake() {
        slider = GetComponent<Slider>();
    }

    public void SetTargetValue(float target) {
        targetValue = target;
    }
    public void SetValue(float value) {
        currentValue = value;
    }

    private void Update() {
        slider.value = currentValue / targetValue;
    }
}
