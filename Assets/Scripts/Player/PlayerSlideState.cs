using UnityEngine;

public class PlayerSlideState : PlayerState
{
    public PlayerSlideState(Player player) : base(player) { }

    private float slideTimer;
    private float slideStopTimer;

    public override void Enter()
    {
        base.Enter();

        anim.SetBool("IsSliding", true);
        player.SetColliderSlide();
        slideTimer = player.slideDuration;
        slideStopTimer = 0;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (slideTimer > 0)
        {
            rb.linearVelocity = new Vector2((player.runSpeed + player.slideSpeedBoost)*SideFacing, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2 (0, rb.linearVelocity.y);
        }
    }
    public override void Update()
    {
        base.Update();

        if (slideTimer > 0)
        {
            slideTimer -= Time.deltaTime;
        }
        else if (slideStopTimer <= 0)
        {
            slideStopTimer = player.slideStopDuration;
        }
        else
        {
            slideStopTimer -= Time.deltaTime;
            if (slideStopTimer <= 0)
            {
                if (player.CheckifCeiling() || MoveInput.y <= -.1f)
                {
                    player.ChangeState(player.crouchState);
                }
                else
                {
                    player.ChangeState(player.idleState);
                }
            }
        }

    }
    public override void Exit()
    {
        base.Enter();

        anim.SetBool("IsSliding", false);
        player.SetColliderNormal();
        
    }
}
