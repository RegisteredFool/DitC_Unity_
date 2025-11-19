using UnityEngine;

public class Enemy : MonoBehaviour
{
    //for the dummy
    public Animator anim;
    public Health health; //health is the script

    [Header("Deaht FX")]
    [SerializeField] private GameObject[] deathParts;
    [SerializeField] private float spawnForce = 5;
    [SerializeField] private float torque = 5;
    [SerializeField] private float lifetime = 5;
    //[SerializeField] private Healthbar healthbar;

    [SerializeField] private int fireStacks = 3;
    private bool onFire;
    private int childFire; //using this to identify which child object the particles are so we always destroy that one 
    [SerializeField] private ParticleSystem fireParticles;

    private void OnEnable()
    {
        health.OnDamaged += HandleDamage;
        health.OnDeath += HandleDeath;
        health.OnFire += HandleFire;
    }

    private void OnDisable()
    {
        health.OnDamaged -= HandleDamage;
        health.OnDeath -= HandleDeath;
        health.OnFire -= HandleFire;
    }

    public void HandleDamage()
    {
        anim.SetTrigger("IsDamaged");
    }
    public void HandleDeath()
    {
        Destroy(gameObject);
        foreach (GameObject prefab in deathParts)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(0.5f, 1)).normalized;
            GameObject part = Instantiate(prefab, transform.position, rotation);

            Rigidbody2D rb = part.GetComponent<Rigidbody2D>();

            Vector2 randomDirection = new Vector2(Random.Range(-1, 1), Random.Range(.5f, 1)).normalized;
            rb.linearVelocity = randomDirection * spawnForce;
            rb.AddTorque(Random.Range(-torque, torque), ForceMode2D.Impulse);
            Destroy(part,lifetime);
        }
    }
    public void HandleFire()
    {
        if (!IsInvoking("HandleFire"))
        {
            if (onFire == false)
            {
                Instantiate(fireParticles, transform);
                childFire = transform.childCount - 1;
                Debug.Log(childFire);
                onFire = true;
                Invoke("HandleFire", 1.5f);
            }
            else if (fireStacks >= 0)
            {
                fireStacks--;
                Invoke("HandleFire", 1.5f);
                health.ChangeHealth(-1, false);
            }
            else if (fireStacks < 0)
            {
                fireStacks = 3;
                health.ChangeHealth(-1, false);
                onFire = false;
                Transform child = transform.GetChild(childFire);
                Destroy(child.gameObject);
            }
        }
        else fireStacks++;
    }
}
