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
    private new BoxCollider2D collider2D = null;

    [SerializeField]
    private float damage = 15;


    private void Awake()
    {

        Physics2D.IgnoreLayerCollision(gameObject.layer, 7, true);
        StartCoroutine(EnableHitPlayer());
    }

    private IEnumerator EnableHitPlayer()
    {
        yield return new WaitForSeconds(timeToShootOut);
        Physics2D.IgnoreLayerCollision(gameObject.layer, 7, false);
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
                Debug.Log("Death!");
            }
        }
    }
}
