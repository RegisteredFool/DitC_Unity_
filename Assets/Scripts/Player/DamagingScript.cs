using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class DamagingScript : MonoBehaviour
{
    [SerializeField] private float radius;
    public float posX;
    public float posY;
    public float posScale;
    public float posScaleY;
    private void Start()
    {
        //transform.position = new Vector2(transform.position.x + posX, transform.position.y + posY);
    }
    public void Damage(LayerMask enemyLayer, Player player, Animator hitFX)
    { 
        Collider2D[] enemy = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayer);
        if (enemy != null)
        {
            int counter = 0;
            hitFX.Play("Active");
            foreach (Collider2D col in enemy)
            {
                if (enemy[counter].gameObject.TryGetComponent<Health>(out var health))
                    health.ChangeHealth(player.dealtDamage * -1, true, Color.white);
                //if (player.damageObjectCurrent == 0)

                    counter++;
            }
            if (player.fire == true)
            {
                counter = 0;
                foreach (Collider2D col in enemy)
                {
                    if (enemy[counter].gameObject.TryGetComponent<Health>(out var health))
                        health.TakeFire();
                    counter++;
                }
            }
        }
    }
}
