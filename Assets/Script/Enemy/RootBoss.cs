using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Like enemy but boss
public class RootBoss : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 100;

    private float currentHealth = 100;
    public float HealthPercentage
    {
        get
        {
            float onePercent = maxHealth / 100;
            return currentHealth / onePercent;
        }
    }

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
    private new Rigidbody2D rigidbody2D = null;

    [SerializeField]
    public float speed = 4;


    private Collider2D playerCollider = null;

    private Action<RootBoss, bool> OnDestroyed = null;

    private bool canCheckDelete = false;

    private Transform transformToFollow = null;

    public void SetUp(Collider2D playerCollider, Transform transformToFollow, Action<RootBoss, bool> OnDestroyed)
    {
        currentHealth = maxHealth;
        this.transformToFollow = transformToFollow;
        this.playerCollider = playerCollider;

        this.collider2D.enabled = false;

        if (playerCollider != null)
            Physics2D.IgnoreCollision(this.collider2D, playerCollider, true);

        StartCoroutine(EnableHitPlayer());
        skeletonMecanim.gameObject.SetActive(false);
        this.OnDestroyed = OnDestroyed;
       
    }

    private IEnumerator EnableHitPlayer()
    {
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


    private void FixedUpdate()
    {
        if (transformToFollow == null)
            return;
        Vector3 direction = transform.position - transformToFollow.position;
        direction = (direction.normalized * speed) * Time.fixedDeltaTime;
        rigidbody2D.AddForce(-direction * rigidbody2D.drag, ForceMode2D.Impulse);
        //transform.position = Vector2.MoveTowards(transform.position, transformToFollow.position, speed * Time.fixedDeltaTime);
    }

    private IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(1f);
        OnDestroyed?.Invoke(this, true);
        Destroy(this.gameObject);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Projectile projectile = collision.gameObject.GetComponent<Projectile>();
        if (projectile != null)
        {
            ScreenShake.Instance.ShakeScreen();
            currentHealth -= projectile.Damage;

            Vector2 force = collision.contacts[0].normal * 50;
            rigidbody2D.AddForceAtPosition(-force, transform.position, ForceMode2D.Impulse);
            if (currentHealth <= 0)
            {
                animator.SetTrigger("despawn");
                this.collider2D.enabled = false;

                if (playerCollider != null)
                    Physics2D.IgnoreCollision(this.collider2D, playerCollider, false);

                AudioSource.PlayClipAtPoint(spawnOutClip, transform.position);
                StartCoroutine(DelayedDestroy());
            }
        }
        else
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                Vector2 force = collision.contacts[0].normal * 100;
                player.Rigidbody2D.AddForceAtPosition(-force, transform.position, ForceMode2D.Impulse);

                player.ReceiveDamage(damage);
            }
            else
            {
                RootEnemy rootEnemy = collision.gameObject.GetComponent<RootEnemy>();
                if (rootEnemy != null)
                {
                    rootEnemy.Kill(false);
                }
            }
        }
    }
}
