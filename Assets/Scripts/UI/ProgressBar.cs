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

    public void SetValues(float value, float maxValue) {
        slider.value = value;
        slider.maxValue = maxValue;
    }
}
