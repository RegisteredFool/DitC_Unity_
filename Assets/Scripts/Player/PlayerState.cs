using UnityEngine;

public abstract class PlayerState
{
    protected Player player;
    protected Animator anim;
    protected Rigidbody2D rb;
    protected Combat combat;
    protected Magic magic;

    protected bool JumpPressed { get => player.jumpPressed; set => player.jumpPressed = value;}
    protected bool JumpReleased { get => player.jumpReleased; set => player.jumpReleased = value;}
    protected bool RunPressed => player.runPressed;
    protected Vector2 MoveInput => player.moveInput;
    protected float SideFacing => player.sideFacing; 
    protected bool AttackPressed => player.attackPressed;
    protected bool SpecialPressed => player.specialPressed;
    //protected float Damage => player.damage;
    protected bool SpellPressed => player.spellPressed;
    public PlayerState(Player player)
    {
        this.player = player;
        this.anim = player.anim;
        this.rb = player.rb;
        combat = player.combat;
        magic = player.magic;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }

    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void AnimationFinished() { }
}
