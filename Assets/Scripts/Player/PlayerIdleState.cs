using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(Player player) : base(player) { }
    public override void Enter()
    {
        anim.SetBool("IsIdle", true);
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }
    public override void Update()
    {
        base.Update();

        if (SpellPressed && magic.canCast)
            player.ChangeState(player.spellcastState);

        else if (AttackPressed && combat.CanAttack)
            player.ChangeState(player.attackState);

        else if (player.jumpPressed)
            player.ChangeState(player.jumpState);

        if (Mathf.Abs(MoveInput.x) > .1)
        {
            player.ChangeState(player.moveState);
        }
        else if (MoveInput.y < -.1f)
        {
            player.ChangeState(player.crouchState);
        }
    }
    public override void Exit()
    {
        base.Exit();
        anim.SetBool("IsIdle", false);
    }
}
