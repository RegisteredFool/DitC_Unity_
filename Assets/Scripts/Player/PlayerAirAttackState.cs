using UnityEngine;

public class PlayerAirAttackState : PlayerState
{
    public PlayerAirAttackState(Player player) : base(player) { }

    public override void Enter()
    {
        base.Enter();
        anim.SetBool("IsJumping", true);
        //Debug.Break();
        //rb.linearVelocity = new Vector2(rb.linearVelocity.x, player.jumpSpeed);
        //SFXManager.instance.PlaySound(player.jumpSFX, player.transform.position, 1f);

        //JumpPressed = false;
        //JumpReleased = false;
    }
    public override void Update()
    {
        base.Update();
        //Debug.Break();
        if (player.isGrounded && rb.linearVelocity.y < .1f)
        {
            player.ChangeState(player.idleState);
        }
        else if (player.jumpPressed && player.jumpsRemaining > 0)
        {
            player.jumpPressed = false;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, player.jumpSpeed * .75f);
            player.jumpsRemaining--;
            if (player.jumpsRemaining == 0)
                SFXManager.instance.PlaySoundAdjust(player.jumpSFX, player.transform.position, 1f, 1.2f);
            else
                SFXManager.instance.PlaySoundAdjust(player.jumpSFX, player.transform.position, 1f, 1f);
        }
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        player.ApplyVariableGravity();
        if (JumpReleased && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * player.jumpCut);
            JumpReleased = false;
        }
        float speed = RunPressed ? player.runSpeed : player.normalSpeed;
        float targetSpeed = speed * MoveInput.x;
        rb.linearVelocity = new Vector2(targetSpeed, rb.linearVelocity.y);
    } 
    public override void AnimationFinished()
    {
        //player.attackPressed = false;
        //if (Mathf.Abs(player.moveInput.x) > .1f)
            //player.ChangeState(player.moveState);
        //else
            //player.ChangeState(player.idleState);
    }
    public override void Exit()
    {
        base.Exit();
        anim.SetBool("IsJumping", false);
        anim.SetBool("IsSpecialing", false);
    }
}
