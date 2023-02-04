using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    private Sprite frontSprite = null;

    [SerializeField]
    private Sprite backSprite = null;

    [SerializeField]
    private new Rigidbody2D  rigidbody2D = null;

    [SerializeField]
    private PlayerStats playerStats;

    [SerializeField]
    private Transform aimTransform = null;

    [SerializeField]
    private Transform gunPoint = null;

    [SerializeField]
    private Weapon weapon;

    [SerializeField]
    private SpriteRenderer spriteRenderer = null;

    [SerializeField]
    private new Collider2D collider2D;
    public Collider2D Collider2D => collider2D;

    [SerializeField]
    private AudioClip playerHurt = null;

    [SerializeField]
    private AudioClip powerUp = null;

    public bool CanMove = false;


    private float currentHealth = 100;
    public float CurrentHealth => currentHealth;

    public float HealthPercentage
    {
        get
        {
            float onePercent = playerStats.maxHealth / 100;
            return CurrentHealth / onePercent;
        }
    }
    private Coroutine flickerCoroutine = null;


    private void Awake()
    {
        currentHealth = playerStats.maxHealth;
    }

    private void Update()
    {
        if (!CanMove)
            return;

        Aim();
        FlipCheck();
        CheckInput();
    }

    private void FixedUpdate()
    {

        if (!CanMove)
            return;

        Movement();
    }

    private void CheckInput()
    {

        float fireTrigger = Input.GetAxis("FireTrigger");
        if (Input.GetButton("Fire1") || fireTrigger > 0)
        {
            Vector3 direction = transform.position - gunPoint.position;
            //Apply bonus here
            float timeBetweenshot = playerStats.timeBetweenShotsMilis;
            if (fireTrigger > 0)
                timeBetweenshot *= 1 + (1-fireTrigger);

            weapon.Shoot(-direction, gunPoint.position, playerStats.bulletCount, playerStats.gunSpread, playerStats.projectileSpeed, playerStats.damage, playerStats.damageOverTime, timeBetweenshot, playerStats.maxBulletRange, playerStats.bulletBounceCount);
        }
    }

    private void FlipCheck()
    {

        Vector2 input = new Vector2(Input.GetAxis("HorizontalRight"), Input.GetAxis("VerticalRight"));
        Vector3 thisScreenPoint = input;
        if (input == Vector2.zero)
        {
            thisScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
            thisScreenPoint = Input.mousePosition - thisScreenPoint;
            
            if (thisScreenPoint.y < 0)
            {
                spriteRenderer.sprite = frontSprite;
            }
            else
            {
                spriteRenderer.sprite = backSprite;
            }
            int flipScale = thisScreenPoint.x > 0 ? 1 : -1;
            spriteRenderer.transform.localScale = new Vector3(flipScale, 1, 1);
            
        }
        else
        {

            if (thisScreenPoint.x > 0)
            {
                spriteRenderer.sprite = frontSprite;
            }
            else
            {
                spriteRenderer.sprite = backSprite;
            }
            int flipScale = thisScreenPoint.y > 0 ? 1 : -1;
            spriteRenderer.transform.localScale = new Vector3(flipScale, 1, 1);
        }

    }

    private void Aim()
    {
        Vector2 input = new Vector2(Input.GetAxis("HorizontalRight"), Input.GetAxis("VerticalRight"));
        Vector3 aimVector = input;
        if (input == Vector2.zero)
        {
            aimVector = Camera.main.WorldToScreenPoint(aimTransform.position);
            aimVector = Input.mousePosition - aimVector;
            gunPoint.transform.localPosition = new Vector3(0, 1, 0);
        }
        else
        {
            gunPoint.transform.localPosition = new Vector3(1, 0, 0);
        }

        float angle = Mathf.Atan2(aimVector.y, aimVector.x) * Mathf.Rad2Deg;
        aimTransform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

    }
    private void Movement()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        input = (input * playerStats.movementSpeed) * Time.fixedDeltaTime;
        //physics way
        rigidbody2D.AddForce(input * rigidbody2D.drag);
        //rigidbody2D.transform.Translate(input);
    }
    public void ReceiveDamage(float damage)
    {
        if (flickerCoroutine != null)
            return;

        currentHealth -= damage;

        AudioSource.PlayClipAtPoint(playerHurt, transform.position);
        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
        else
        {
            flickerCoroutine = StartCoroutine(Flicker());
        }
    }

    private IEnumerator Flicker()
    {
        for (int i = 0; i < 6; i++)
        {
            spriteRenderer.color = Color.clear;
            yield return new WaitForSeconds(0.15f);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.15f);
        }
        flickerCoroutine = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BonusPickable bonusPickable = collision.gameObject.GetComponent<BonusPickable>();
        if (bonusPickable != null)
        {

            ApplyBonus(bonusPickable);
            Destroy(bonusPickable.gameObject);
        }
    }

    private void ApplyBonus(BonusPickable bonusPickable)
    {
        AudioSource.PlayClipAtPoint(powerUp, bonusPickable.transform.position);
        //Do magic bonus shit
        playerStats.bulletCount += bonusPickable.addBulletCount;
        playerStats.damage += bonusPickable.addDamage;
        playerStats.gunSpread += bonusPickable.addSpread;
        playerStats.timeBetweenShotsMilis -= bonusPickable.reduceTimeBetweenShotsMilis;
        playerStats.bulletBounceCount += bonusPickable.addBounceCount;
        currentHealth += bonusPickable.addHealth;
        playerStats.movementSpeed += bonusPickable.addMovementSpeed;
        playerStats.projectileSpeed += bonusPickable.addProjectileSpeed;
        playerStats.damageOverTime += bonusPickable.addDamageOverTime;
        playerStats.maxBulletRange += bonusPickable.addMaxBulletRange;

        if (playerStats.damage < 1)
            playerStats.damage = 1;
    }
}
