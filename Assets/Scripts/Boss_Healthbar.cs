using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss_Healthbar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetMaxHealth(int health){ 
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f); //grab color at specific point // grab value from 0 to 1
    }

    public void SetHealth(int health) { 
        slider.value = health;

        fill.color =  gradient.Evaluate(slider.normalizedValue);
    }
}
