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
    private ParticleSystem spawnParticle = null;

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

    [SerializeField]
    private bool canMove = false;

    [SerializeField]
    private float timeOffScreenBeforeDying = 10;

    private float destroyTimer = 0;


    private Collider2D playerCollider = null;

    private Action<RootEnemy, bool> OnDestroyed = null;

    private bool canCheckDelete = false;

    public void SetUp(Collider2D playerCollider, Action<RootEnemy, bool> OnDestroyed)
    {
        this.playerCollider = playerCollider;

        this.collider2D.enabled = false;

        if(playerCollider != null)
            Physics2D.IgnoreCollision(this.collider2D, playerCollider, true);

        StartCoroutine(EnableHitPlayer());
        skeletonMecanim.gameObject.SetActive(false);
        this.OnDestroyed = OnDestroyed;
    }

    private IEnumerator EnableHitPlayer()
    {
        /*
        Color start = spriteRenderer.color;
        spriteRenderer.color = Color.clear;
        while (spriteRenderer.color != start)
        {
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, start, timeToShootOut * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        spriteRenderer.color = start;
        spriteRenderer.gameObject.SetActive(false);
        */
        spawnParticle.Play();
        yield return new WaitForSeconds(spawnParticle.main.duration);
        spawnParticle?.Stop();
        spawnParticle.gameObject.SetActive(false);

        skeletonMecanim.gameObject.SetActive(true);
        animator.SetTrigger("spawn");
        AudioSource.PlayClipAtPoint(spawnInClip, transform.position);

        this.collider2D.enabled = true;
        //yield return new WaitForSeconds(timeToShootOut);
        if (playerCollider != null)
            Physics2D.IgnoreCollision(this.collider2D, playerCollider, false);

        yield return new WaitForSeconds(1);
        canCheckDelete = true;
    }

    
    private void Update()
    {
        if (canMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, FindObjectOfType<Player>().transform.position, 4f * Time.deltaTime);
        }
        else
        {
            if (!meshRenderer.isVisible && skeletonMecanim.gameObject.activeInHierarchy
                 && !spawnParticle.gameObject.activeInHierarchy && canCheckDelete)
            {
                destroyTimer += Time.deltaTime;
                if (destroyTimer > timeOffScreenBeforeDying)
                {
                    OnDestroyed?.Invoke(this, false);
                    Destroy(this.gameObject);
                }
            }
            else
            {
                destroyTimer = 0;
            }
        }
    }

    private IEnumerator DelayedDestroy(bool player)
    {
        yield return new WaitForSeconds(1f);
        OnDestroyed?.Invoke(this, player);
        Destroy(this.gameObject);

    }
    public void Kill(bool player)
    {
        animator.SetTrigger("despawn");
        this.collider2D.enabled = false;

        if (playerCollider != null)
            Physics2D.IgnoreCollision(this.collider2D, playerCollider, false);

        AudioSource.PlayClipAtPoint(spawnOutClip, transform.position);
        StartCoroutine(DelayedDestroy(player));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Projectile projectile = collision.gameObject.GetComponent<Projectile>();
        if (projectile != null)
        {
            health -= projectile.Damage;
            ScreenShake.Instance.ShakeScreen();

            if (health <= 0)
            {
                Kill(true);
            }
        }
        else
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                if (canMove)
                {
                    Vector2 force  = collision.contacts[0].normal * 150;
                    Debug.Log(force);
                    player.Rigidbody2D.AddForceAtPosition(-force, transform.position,ForceMode2D.Impulse);
                }
                player.ReceiveDamage(damage);
            }
        }
    }
}
