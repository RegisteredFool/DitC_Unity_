using NUnit.Framework.Internal.Filters;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class Combat : MonoBehaviour
{
    public Player player;
    public Animator hitFX;
    private AudioSource audioSource;
    [SerializeField] private AudioClip hitSFX;
    [SerializeField] private float vol;


    [Header("Atack Settings")]
    public LayerMask enemyLayer;
    public float damage;
    public Transform attackPoint;
    public float attackRadius = .5f;
    public float attackCooldown = 1.5f;
    public bool attackPressed;
    public bool CanAttack => Time.time >= nextAttackTime;
    private float nextAttackTime;

    public bool canSpell;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public void AttackAnimationFinished()
    {
        player.AnimationFinished();
    }

    public void Attack()
    {
        SFXManager.instance.PlaySound(hitSFX, transform.position, vol);
        // if (!CanAttack)
        //return;
        nextAttackTime = Time.time + attackCooldown;

        Collider2D[] enemy = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayer);
        if (enemy != null)
        {
            int counter = 0;
            foreach (Collider2D col in enemy)
            {
                hitFX.Play("Active");
                if (enemy[counter].gameObject.TryGetComponent<Health>(out var health))
                    health.ChangeHealth(-damage, true);
                counter++;
            }
            if (player.fire == true)
            {
                counter = 0;
                foreach (Collider2D col in enemy)
                {
                    if (enemy[counter].gameObject.TryGetComponent<Health>(out var health))
                        health.TakeFire();
                    counter++;
                }
            }
        }
    }
    public void Interact()
    {
        Collider2D interact = Physics2D.OverlapCircle(transform.position, 5, enemyLayer);
        if (interact != null)
        {
            interact.GetComponent<DialogueManager>().BeginDialogue();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
