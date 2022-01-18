using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_MagicBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public Text levelText;
    // [HideInInspector]
    public int magicbarLvl;

    public void SetMaxMagic(float magic){ //shortcut to set magic bar at max
        slider.maxValue = magic;
        slider.value = magic;

        fill.color = gradient.Evaluate(1f); //grab color at specific point // grab value from 0 to 1
    }

    public void SetMagic(float magic) { //set magic value and update slider
        slider.value = magic;

        fill.color =  gradient.Evaluate(slider.normalizedValue);
    }

    public void SetMagicLevel(int level) { //sets magic level
        magicbarLvl = level;
        levelText.text = magicbarLvl.ToString();
    }
}
