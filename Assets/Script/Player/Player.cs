using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
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
        Aim();
        FlipCheck();
        CheckInput();
    }

    private void FixedUpdate()
    {
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

            weapon.Shoot(-direction,gunPoint.position, playerStats.bulletCount, playerStats.gunSpread, playerStats.projectileSpeed, playerStats.damage, playerStats.damageOverTime, timeBetweenshot,playerStats.maxBulletRange);
        }
    }

    private void FlipCheck()
    {
        Vector3 thisScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
        thisScreenPoint = Input.mousePosition - thisScreenPoint;
        int flipScale = thisScreenPoint.x > 0 ? 1 : -1;
        transform.localScale = new Vector3(flipScale, 1, 1);
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
            //Do magic bonus shit

            playerStats.bulletCount += bonusPickable.addBulletCount;
            playerStats.damage += bonusPickable.addDamage;
            playerStats.gunSpread += bonusPickable.addSpread;

            Destroy(bonusPickable.gameObject);
        }
    }
}
