using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Statistics {
public class Energy : MonoBehaviour {
    [Header("Values")]
    public float maxEnergy = 1000;
    [HideInInspector] public float currentEnergy;
    public float regenPerSecond = 4f;
    public float regenDelay = 5f;
    
    [Header("UI")]
    public bool hasEnergyBar;
    public Slider slider;
    public Image energyFill;
    public TextMeshProUGUI energyText;
    public Gradient energyColour;
    
    [HideInInspector] public float timeSinceUse;
    [HideInInspector] public bool isRewinding;
    
    private void Start() {
        SetEnergyMax();
    }
    
    private void Update() {
        if (!isRewinding) {
            timeSinceUse += Time.deltaTime;
            if (currentEnergy < maxEnergy && timeSinceUse > regenDelay)
            {
                Recover(regenPerSecond * Time.deltaTime);
            }
        }
    }
    
    private void SetEnergyMax() {
        if (hasEnergyBar)
        {
            slider.maxValue = maxEnergy;
        }
        currentEnergy = maxEnergy;
        SetEnergySlider();
    }

    public void SetEnergySlider() {
        if (hasEnergyBar)
        {
            slider.value = currentEnergy;
            energyText.text = Mathf.Floor(currentEnergy) + "";
            energyFill.color = energyColour.Evaluate(slider.normalizedValue);
        }
    }
    
    public void Expend(float value) {
        if (Mathf.Abs(value) > 0 && !isRewinding) { 
            timeSinceUse = 0; 
            currentEnergy = Mathf.Max(0, currentEnergy - value);
            SetEnergySlider();
        }
    }
    
    public float ExpendQuery(float value) {
        return Mathf.Max(0, currentEnergy - value);
    }
    
    public void Recover(float value) {
        if (Mathf.Abs(value) > 0 && !isRewinding) {
            currentEnergy = Mathf.Min(maxEnergy, currentEnergy + value);
            SetEnergySlider();
        }
    }
}
}
