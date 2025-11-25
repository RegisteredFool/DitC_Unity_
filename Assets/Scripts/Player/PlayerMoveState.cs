using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(Player player) : base(player) { }

    public override void Enter()
    {
        base.Enter();

        anim.SetBool("IsWalking", false);
        anim.SetBool("IsRunning", false);
    }

    public override void Update()
    {
        base.Update();

        if (SpellPressed && magic.canCast)
            player.ChangeState(player.spellcastState);

        else if (AttackPressed && combat.CanAttack)
        {
            player.ChangeState(player.attackState);
            player.dealtDamage = player.damage[0];
            player.damageObjectCurrent = 0;
            anim.SetBool("IsAttacking", true);
        }
        else if (SpecialPressed && combat.CanAttack)
        {
            player.ChangeState(player.specialState);
            player.dealtDamage = player.damage[1];
            player.damageObjectCurrent = 1;
            anim.SetBool("IsSpecialing", true);
        }

        else if (JumpPressed)
            player.ChangeState(player.jumpState);

        else if (Mathf.Abs(MoveInput.x) < .1f) 
            player.ChangeState(player.idleState);
        else if(player.isGrounded && RunPressed && MoveInput.y <= -.1f)
            player.ChangeState(player.slideState);
        else
        {
            anim.SetBool("IsWalking", !RunPressed);
            anim.SetBool("IsRunning", RunPressed);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        float speed = RunPressed ? player.runSpeed : player.normalSpeed;
        rb.linearVelocity = new Vector2(speed * player.sideFacing, rb.linearVelocity.y);

    }

    public override void Exit()
    {
        base.Exit();
        anim.SetBool("IsWalking", false);
        anim.SetBool("IsRunning", false);
    }

}
