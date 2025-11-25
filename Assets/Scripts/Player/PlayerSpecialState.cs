using UnityEngine;

public class PlayerSpecialState : PlayerState
{
    public PlayerSpecialState(Player player) : base(player) { }
    public override void Enter()
    {
        base.Enter();

        anim.SetBool("IsSpecialing", true);
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        player.dealtDamage = player.damage[1];
        player.damageObjectCurrent = 1;
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (player.moveInput.x != 0)
        {
            float speed = RunPressed ? player.runSpeed : player.normalSpeed;
            rb.linearVelocity = new Vector2(speed/2 * player.sideFacing, rb.linearVelocity.y);
        }
        else
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
        anim.SetBool("IsSpecialing", false);
    }
}
