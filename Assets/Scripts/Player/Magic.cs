using UnityEngine;

public class Magic : MonoBehaviour
{
    public Player player;
    public LayerMask terrain;
    public float spellRange;
    public float castCooldown;

    public float playerRadius = 1.5f;

    private float nextCastTime;
    public bool canCast => Time.time >= nextCastTime;
    public void AnimationFinished()
    {
        player.AnimationFinished();
    }

    public void CastSpell()
    {
        Teleport();
    }

    private void Teleport()
    {
        Vector2 direction = new Vector2(player.sideFacing, 0);
        Vector2 targetPosition = (Vector2)player.transform.position + direction * spellRange;

        Collider2D hit = Physics2D.OverlapCircle(targetPosition, playerRadius, terrain);
        if (hit != null)
        {
            float step = .1f;
            Vector2 adjustedPosition = targetPosition;
            while (hit != null && Vector2.Distance(adjustedPosition, player.transform.position) > 0)
            {
                adjustedPosition -= direction * step;
                hit = Physics2D.OverlapCircle(adjustedPosition, playerRadius, terrain);
            }
            targetPosition = adjustedPosition;
        }

        player.transform.position = targetPosition;
        nextCastTime = Time.time + castCooldown;
    }
}
