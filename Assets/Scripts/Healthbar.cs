using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI numericalHealth;
    public void UpdateHealthbar(float currentValue, float maxValue)
    {
        healthBar.value = currentValue / maxValue;
        //numericalHealth.text = "HP " + currentValue + "/" + maxValue;
        Debug.Log(currentValue);
    }
}
