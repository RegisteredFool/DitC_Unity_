using UnityEngine;

public class PlayerSpellcastState : PlayerState
{
    public PlayerSpellcastState(Player player) : base(player) { }

    public override void Enter()
    {
        base.Enter();
        anim.SetBool("IsSpellcasting", true);
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    public override void AnimationFinished()
    {
        base.AnimationFinished();
        if (Mathf.Abs(MoveInput.x) > .1f)
            player.ChangeState(player.moveState);
        else
            player.ChangeState(player.idleState);
    }

    public override void Exit()
    {
        base.Enter();
        Debug.Log(player.spellPressed);
        anim.SetBool("IsSpellcasting", false);
    }
}
