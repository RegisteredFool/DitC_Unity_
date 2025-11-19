using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player player) : base(player) { }

    public override void Enter()
    {
        base.Enter();
        anim.SetBool("IsJumping", true);

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, player.jumpSpeed);
        SFXManager.instance.PlaySound(player.jumpSFX, player.transform.position, 1f);

        JumpPressed = false;
        //JumpReleased = false;
    }
    public override void Update()
    {
        base.Update();
        if (player.isGrounded && rb.linearVelocity.y < .1f)
        {
            player.ChangeState(player.idleState);
        }
        else if (player.jumpPressed && player.jumpsRemaining > 0)
        {
            player.jumpPressed = false;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, player.jumpSpeed*.75f);
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
    public override void Exit()
    {
        base.Exit();
        anim.SetBool("IsJumping", false);
    }

    /*
     * private void HandleJump()
    {
        if (jumpPressed && jumpsRemaining != 0)
        {
            if (isGrounded == true) rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpSpeed);
            else rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpSpeed*.5f);
            jumpsRemaining--;
            jumpPressed = false;
            jumpReleased = false;
        }
        if (jumpReleased) //still going up
        {
            if (rb.linearVelocity.y > 0) rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCut);
            jumpReleased = true;
        }
    }
    */
}
