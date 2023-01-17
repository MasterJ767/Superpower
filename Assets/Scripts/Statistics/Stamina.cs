using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Statistics {
public class Stamina : MonoBehaviour {
    [Header("Values")]
    public float maxStamina = 1000;
    [HideInInspector] public float currentStamina;
    public float regenPerSecond = 4f;
    public float regenDelay = 5f;
    
    [Header("UI")]
    public bool hasStaminaBar;
    public Slider slider;
    public Image staminaFill;
    public TextMeshProUGUI staminaText;
    public Gradient staminaColour;
    
    private float timeSinceUse;

    private void Start()
    {
        SetStaminaMax();
    }
    
    private void Update()
    {
        timeSinceUse += Time.deltaTime;
        if (currentStamina < maxStamina && timeSinceUse > regenDelay)
        {
            Recover(regenPerSecond * Time.deltaTime);
        }
    }
    
    private void SetStaminaMax()
    {
        if (hasStaminaBar)
        {
            slider.maxValue = maxStamina;
        }
        currentStamina = maxStamina;
        SetStaminaSlider();
    }

    private void SetStaminaSlider()
    {
        if (hasStaminaBar)
        {
            slider.value = currentStamina;
            staminaText.text = Mathf.Floor(currentStamina) + "";
            staminaFill.color = staminaColour.Evaluate(slider.normalizedValue);
        }
    }
    
    public void Expend(float value)
    {
        timeSinceUse = 0;
        currentStamina = Mathf.Max(0, currentStamina - value);
        SetStaminaSlider();
    }
    
    public float ExpendQuery(float value)
    {
        return Mathf.Max(0, currentStamina - value);
    }
    
    public void Recover(float value)
    {
        currentStamina = Mathf.Min(maxStamina, currentStamina + value);
        SetStaminaSlider();
    }
}
}