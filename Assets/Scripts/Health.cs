using System;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public event Action OnDamaged;
    public event Action OnDeath;
    public event Action OnFire;

    public float health;
    public float maxHealth;

    //[SerializeField] private Healthbar healthbar;

    [SerializeField] private Slider healthBar;
    [SerializeField] private GameObject damageNumber;
    [SerializeField] private float damageNumberOffset; //only works in y 
    [SerializeField] private float damageNumberLifespan;
    [SerializeField] private AudioClip hitSFX;
    private AudioSource audioSource;
    public void UpdateHealthbar(float currentValue, float maxValue)
    {
        healthBar.value = currentValue / maxValue;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        health = maxHealth;
        UpdateHealthbar(health, maxHealth);
    }
    public void ChangeHealth(float amount, bool flinch) //consider adding variables that dictate what color will be used and if it should cause a hurt animation, and maybe a sprite flash too
    {
        Debug.Log("Damage Happen");
        //gSFXManager.instance.PlaySound(hitSFX, transform.position, 1);
        health += amount;

        float ex = transform.position.x + (UnityEngine.Random.Range(-0.75f,0.75f)); float why = transform.position.y + (UnityEngine.Random.Range(-0.75f, 0.75f) + damageNumberOffset);
        Vector2 pos = new Vector2(ex, why+4);
        damageNumber.GetComponent<TMP_Text>().text = (Mathf.Abs(amount)).ToString();
        GameObject thatNumber = Instantiate(damageNumber, pos, Quaternion.identity); 
        Destroy(thatNumber, damageNumberLifespan);

        if (health > maxHealth)
            health = maxHealth;
        else if (health < 0)
        {
            OnDeath?.Invoke();
        }
        else if (amount < 0 && flinch == true)
        {
            OnDamaged?.Invoke();
        }
        UpdateHealthbar(health, maxHealth);
    }
    public void TakeFire()
    {
        OnFire?.Invoke();
    }
    public void Test()
    {
        Debug.Log("Does");
    }
}
