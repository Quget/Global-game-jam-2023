using Spine.Unity;
using System;
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

    [SerializeField]
    private Animator animator = null;

    [SerializeField]
    private SkeletonMecanim skeletonMecanim = null;

    [SerializeField]
    private AudioClip spawnInClip = null;

    [SerializeField]
    private AudioClip spawnOutClip = null;

    [SerializeField]
    private MeshRenderer meshRenderer = null;

    private Collider2D playerCollider = null;

    private Action<RootEnemy> OnDestroyed = null;
    public void SetUp(Collider2D playerCollider, Action<RootEnemy> OnDestroyed)
    {
        this.playerCollider = playerCollider;

        this.collider2D.enabled = false;
        Physics2D.IgnoreCollision(this.collider2D, playerCollider, true);
        StartCoroutine(EnableHitPlayer());
        skeletonMecanim.gameObject.SetActive(false);
        this.OnDestroyed = OnDestroyed;
    }

    private IEnumerator EnableHitPlayer()
    {
        Color start = spriteRenderer.color;
        spriteRenderer.color = Color.clear;
        while (spriteRenderer.color != start)
        {
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, start, timeToShootOut * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        spriteRenderer.color = start;
        spriteRenderer.gameObject.SetActive(false);
        skeletonMecanim.gameObject.SetActive(true);
        animator.SetTrigger("spawn");
        AudioSource.PlayClipAtPoint(spawnInClip, transform.position);

        this.collider2D.enabled = true;
        //yield return new WaitForSeconds(timeToShootOut);
        Physics2D.IgnoreCollision(this.collider2D, playerCollider, false);
    }

    private void OnDestroy()
    {
        OnDestroyed?.Invoke(this);
    }
    private void FixedUpdate()
    {
        if (!meshRenderer.isVisible && skeletonMecanim.gameObject.activeInHierarchy)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Projectile projectile = collision.gameObject.GetComponent<Projectile>();
        if (projectile != null)
        {
            health -= projectile.Damage;
            if (health <= 0)
            {
                animator.SetTrigger("despawn");
                this.collider2D.enabled = false;
                Physics2D.IgnoreCollision(this.collider2D, playerCollider, false);
                AudioSource.PlayClipAtPoint(spawnOutClip, transform.position);
                Destroy(this.gameObject, 1f);
            }
             
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
