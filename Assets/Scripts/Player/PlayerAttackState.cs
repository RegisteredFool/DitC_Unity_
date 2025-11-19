using UnityEngine;

public class PlayerAttackState : PlayerState
{
    public PlayerAttackState(Player player) : base(player) { }

    public override void Enter()
    {
        base.Enter();

        anim.SetBool("IsAttacking", true);
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    public override void AnimationFinished()
    {
        //player.attackPressed = false;
        if (Mathf.Abs(player.moveInput.x) > .1f)
            player.ChangeState(player.moveState);
        else
            player.ChangeState(player.idleState);
    }
    
    public override void Exit()
    {
        base.Exit();
        anim.SetBool("IsAttacking", false);
    }
}
