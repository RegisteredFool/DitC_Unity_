using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Slider healthBar;

    public void UpdateHealthbar(float currentValue, float maxValue)
    {
        healthBar.value = currentValue / maxValue;
    }
}
