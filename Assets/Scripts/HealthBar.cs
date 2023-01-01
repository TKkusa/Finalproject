using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetHealth(int health) {
        slider.value = health;
        switch (health) {
            case 4: case 5:
                fill.color = gradient.Evaluate(1f);
                break;
            case 2: case 3:
                fill.color = gradient.Evaluate(0.5f);
                break;
            case 1: case 0:
                fill.color = gradient.Evaluate(0.1f);
                break;
        }
    }

}
