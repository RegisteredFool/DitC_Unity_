using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class Player : MonoBehaviour
{
    #region
    public PlayerState currentState;

    public PlayerIdleState idleState;
    public PlayerJumpState jumpState;
    public PlayerMoveState moveState;
    public PlayerCrouchState crouchState;
    public PlayerSlideState slideState;
    public PlayerAttackState attackState;
    public PlayerSpecialState specialState;
    public PlayerSpellcastState spellcastState;
    public PlayerAirAttackState airAttackState;

    public float maxHealth = 20.0f;
    public float health = 20.0f;
    [SerializeField] private Healthbar playerHealthbar; 
    public Vector2 moveInput;
    public float normalSpeed = 2.0f;
    public float runSpeed = 3.0f;
    public float currentSpeed;
    public bool runPressed;
    public float sideFacing = 1;
    public float[] damage;
    public GameObject[] damageObjects;
    public int damageObjectCurrent;
    public float dealtDamage = 3;

    [Header("Core Components")]
    public Health playerHealth;
    public Combat combat;
    public Magic magic;

    [Header("Components")]
    public PlayerInput playerInput;
    public Rigidbody2D rb;
    public CapsuleCollider2D playerCollider;
    public Animator anim;

    [Header("Jump Variables")]
    public float jumpSpeed;
    public float jumpCut = 0.5f;
    public float normalGravity;
    public float fallGravity;
    public float jumpGravity;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    public int jumpMax;
    public int jumpsRemaining;
    public bool jumpPressed;
    public bool jumpReleased;
    public bool isGrounded;

    [Header("Crouch Check")]
    public Transform headCheck;
    public float headCheckRadius = .2f;

    [Header("Slide Settings")]
    public float slideDuration = .6f;
    public float slideSpeedBoost = 3.0f;
    public float slideStopDuration = .15f;

    public float slideHeight;
    public Vector2 slideOffset;
    public float normalHeight;
    public Vector2 normalOffset;

    private bool isSliding;

    public bool attackPressed;
    public bool specialPressed;
    public bool spellPressed;

    [Header("Fire")]
    public bool fire;
    [SerializeField] private ParticleSystem fireParticles;
    private int fireStacks = -1;

    [Header("Sounds")]
    public AudioClip jumpSFX;
    public float pitch;
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        idleState = new PlayerIdleState(this);
        jumpState = new PlayerJumpState(this);
        moveState = new PlayerMoveState(this);
        crouchState = new PlayerCrouchState(this);
        slideState = new PlayerSlideState(this);
        attackState = new PlayerAttackState(this);
        specialState = new PlayerSpecialState(this);
        spellcastState = new PlayerSpellcastState(this);
        airAttackState = new PlayerAirAttackState(this);
    }
    void Start()
    {
        jumpsRemaining = jumpMax;
        rb.gravityScale = normalGravity;
        currentSpeed = normalSpeed;
        ChangeState(idleState);
        playerHealthbar.UpdateHealthbar(health, maxHealth);

    }
    private void OnEnable()
    {
        playerHealth.OnDamaged += HandleDamage;
    }
    private void OnDisable()
    {
        playerHealth.OnDamaged -= HandleDamage;
    }
    void HandleDamage()
    {
        //Debug.Log("Yay");
    }
    public void ChangeState (PlayerState newState)
    {
        if (currentState != null) 
            currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
    private void FixedUpdate()
    {
        currentState.FixedUpdate();
        CheckGrounded();
    }
    void Update()
    {
        currentState.Update();
        if (moveInput.x != 0 && !isSliding)
        {
            sideFacing = Math.Sign(moveInput.x);
            transform.localScale = new Vector2(sideFacing, 1);
        }
        HandleAnimations();
    }
    public void AnimationFinished()
    {
        currentState.AnimationFinished();
    }
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    public void OnAttack(InputValue value)
    {
        attackPressed = value.isPressed;
    }
    public void OnSpecial(InputValue value)
    {
        specialPressed = value.isPressed;
    }
    public void OnSpellcast(InputValue value)
    {
        spellPressed = value.isPressed;
    }
    public void OnInteract(InputValue value)
    {
        combat.Interact();
    }
    public void CheckifGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
    public bool CheckifCeiling()
    {
        return Physics2D.OverlapCircle(headCheck.position, headCheckRadius, groundLayer);
    }

    public void SetColliderNormal()
    {
        playerCollider.size = new Vector2(playerCollider.size.x, normalHeight);
        playerCollider.offset = normalOffset;
    }
    public void SetColliderSlide()
    {
        playerCollider.size = new Vector2(playerCollider.size.x, slideHeight);
        playerCollider.offset = slideOffset;
    }
    void HandleAnimations()
    {
        anim.SetBool("IsGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
    }
    public void OnDash(InputValue value)
    {
        runPressed = value.isPressed;
    }
    public void CheckGrounded()
    {
        if (Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer) == true)
        {
            jumpsRemaining = jumpMax;
            isGrounded = true;
            anim.SetBool("IsGrounded", true);
        } else isGrounded = false;
    }
    public void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            jumpPressed = true;
            jumpReleased = false;
        }
        else
        {
            jumpPressed = false;
            jumpReleased = true;
        }
    }
    public void ApplyVariableGravity()
    {
        if (rb.linearVelocity.y < -0.1f)
        {
            rb.gravityScale = fallGravity;
        }
        else if (rb.linearVelocity.y > 0.1f)
        {
            rb.gravityScale = jumpGravity;
        }
        else
        {
            rb.gravityScale = normalGravity;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(headCheck.position, headCheckRadius);
        Gizmos.color = Color.green;
        //Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
    public void CutsceneIdle()
    {
        moveInput.x = 0;
        runPressed = false;
    }
    public void BeginFire()
    {
        if (fire != true)
        {
            Instantiate(fireParticles, transform);
            fire = true;
        }
    }
    public void TakeFire()
    {
        if (fireStacks == -1)
        {
            Invoke("TakeFire", 1.5f);
            fireStacks = 2;
        }
        else if (fireStacks != 0)
        {
            Invoke("TakeFire", 1.5f);
            fireStacks--;
            playerHealth.ChangeHealth(-1, false, Color.red);
        }
        else if (fireStacks == 0)
        {
            fireStacks--;
            playerHealth.ChangeHealth(-1, false, Color.red);
        }
        //Debug.Log(health);
    }
}
