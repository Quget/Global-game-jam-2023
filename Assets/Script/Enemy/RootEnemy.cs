using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootEnemy : MonoBehaviour
{
    [SerializeField]
    private float health = 100;

    [SerializeField]
    private float timeToShootOut = 5;

    [SerializeField]
    private new Collider2D collider2D = null;

    [SerializeField]
    private float damage = 15;

    [SerializeField]
    private SpriteRenderer spriteRenderer = null;

    private Collider2D playerCollider = null;
    public void SetUp(Collider2D playerCollider)
    {
        this.playerCollider = playerCollider;
        spriteRenderer.color = Color.clear;
        Physics2D.IgnoreCollision(this.collider2D, playerCollider, true);
        StartCoroutine(EnableHitPlayer());
    }

    private IEnumerator EnableHitPlayer()
    {
        while (spriteRenderer.color != Color.white)
        {
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, Color.white, timeToShootOut * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        spriteRenderer.color = Color.white;
        //yield return new WaitForSeconds(timeToShootOut);
        Physics2D.IgnoreCollision(this.collider2D, playerCollider, false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Projectile projectile = collision.gameObject.GetComponent<Projectile>();
        if (projectile != null)
        {
            health -= projectile.Damage;
            if (health <= 0)
                Destroy(this.gameObject);
        }
        else
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.ReceiveDamage(damage);
            }
        }
    }
}
