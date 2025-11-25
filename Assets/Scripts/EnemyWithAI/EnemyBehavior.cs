using System.ComponentModel.Design;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Unity.UI;

public class EnemyBehavior : MonoBehaviour
{
    #region
    public Health health;
    public Transform rayCast;
    public Canvas canvas;
    public LayerMask raycastMask;
    public float raycastLength;
    public float attackDistance;
    public float moveSpeed;
    public float timer;

    [Header ("Attack")]
    public LayerMask playerLayer;
    public float damage;
    public Transform attackPoint;
    public float attackRadius = .5f;

    private RaycastHit2D hit;
    private GameObject target;
    private Animator anim;
    private float distance;
    private bool attackMode;
    private bool inRange;
    private bool cooling;
    private float intTimer;
    private int sideFacing;
    private bool damaged = false;

    private bool onFire = false;
    private int childFire;
    public int fireStacks = -1;
    [SerializeField] private ParticleSystem fireParticles;
    #endregion

    private void Awake()
    {
        intTimer = timer;
        anim = GetComponent<Animator>();
    }
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
    void Update()
    {
        if (inRange)
        {
            hit = Physics2D.Raycast(rayCast.position, Vector2.left, raycastLength, raycastMask);
            RaycastDebugger();
            EnemyLogic();
        }

        if(hit.collider != null)
        {
        }
        else
        {
            //inRange = false;
            //anim.SetBool("IsIdle", true);
            //anim.SetBool("IsWalking", false);
        }
        if (inRange == false)
        {
            anim.SetBool("IsWalking", false);
            StopAttack();
        }
    }

    void EnemyLogic()
    {
        if (damaged == true)
            return;
        distance = Vector2.Distance(transform.position, target.transform.position);
        if (distance > attackDistance)
        {
            DoMove();
            StopAttack();
        }
        else if (attackDistance >= distance && cooling == false)
        {
            Attack();
        }
    }
    void Attack()
    {
        if (damaged == true)
            return;
        timer = intTimer;
        attackMode = true;

        anim.SetBool("IsWalking", false);
        anim.SetBool("IsIdle", false);
        anim.SetBool("IsAttacking", true);
    }
    void StopAttack()
    {
        cooling = false;
        attackMode = false;
        anim.SetBool("IsAttacking", false);
    }
    void DoMove()
    {
        if (damaged == true)
            return; 
        //Debug.Log("Distance > attack");
        anim.SetBool("IsWalking", true);
        anim.SetBool("IsIdle", false);
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            Vector2 targetPosition = new Vector2(target.transform.position.x, transform.position.y);
            //anim.SetBool("IsIdle", false);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, 2 * Time.deltaTime);
            
            sideFacing = (int)Mathf.Sign(transform.position.x - targetPosition.x);
            transform.localScale = new Vector3(7.7396f * sideFacing, 7.7396f, 7.7396f);
        }
    }
    void RaycastDebugger()
    {
        if (distance > attackDistance)
        {
            Debug.DrawRay(rayCast.position, Vector2.left * raycastLength, Color.red);
        }
        else
        {
            Debug.DrawRay(rayCast.position, Vector2.left * raycastLength, Color.green);
        }
    }
    private void OnTriggerEnter2D(Collider2D player)
    {
        if (player.gameObject.tag == "Player")
        {
            target = player.gameObject;
            inRange = true;
        }
    }
    public void HandleDamage()
    {
        anim.SetBool("IsIdle", false);
        anim.SetBool("IsWalking", false);
        anim.SetBool("IsAttacking", false);
        anim.SetBool("IsDamaged", true);
        
        //anim.SetBool("IsAttacking", false);
        StopAttack();
        damaged = true;
    }
    public void StopDamage()
    {
        damaged = false;
        anim.SetBool("IsIdle", true);
        anim.SetBool("IsDamaged", false);
    }
    public void Test()
    {
        Debug.Log("Does");
    }
    public void HandleDeath()
    {
        anim.SetTrigger("Die");
        //Destroy(gameObject);
    }
    public void HandleFire()
    {
        if (!IsInvoking("HandleFire"))
        {
            if (onFire == false && fireStacks == -1)
            {
                Instantiate(fireParticles, transform);
                onFire = true;
                childFire = transform.childCount - 1; //PROBLEM! What happens if, in between now and the destruction of this child object, a child object spawned before this one is destroyed?
                fireStacks = 3;
                Invoke("HandleFire", 1.5f);
            }
            else if (fireStacks > 0 && onFire == true)
            {
                fireStacks--;
                Invoke("HandleFire", 1.5f);
                health.ChangeHealth(-1, false, Color.red);
                //health.health--;
                //health.UpdateHealthbar(health.health, health.maxHealth);
            }
            else if (fireStacks == 0 && onFire == true)
            {
                health.ChangeHealth(-1, false, Color.red);
                onFire = false;
                fireStacks--;
                Transform child = transform.GetChild(childFire);
                Destroy(child.gameObject);
            }
        }
        else fireStacks++;
    }
    public void Attacking()
    {
        Collider2D player = Physics2D.OverlapCircle(attackPoint.position, attackRadius, playerLayer);
        if (player != null)
        {
            player.gameObject.GetComponent<Health>().ChangeHealth(-damage, true, Color.white);
            if (onFire == true)
            {
                player.gameObject.GetComponent<Player>().TakeFire();
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
