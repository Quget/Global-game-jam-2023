using Spine.Unity;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    private new Rigidbody2D  rigidbody2D = null;
    public Rigidbody2D Rigidbody2D => rigidbody2D;

    [SerializeField]
    private PlayerStats playerStats;

    [SerializeField]
    private Transform aimTransform = null;

    [SerializeField]
    private Transform gunPoint = null;

    [SerializeField]
    private Weapon weapon;

    [SerializeField]
    private new Collider2D collider2D;
    public Collider2D Collider2D => collider2D;

    [SerializeField]
    private AudioClip playerHurt = null;

    [SerializeField]
    private AudioClip powerUp = null;

    [SerializeField]
    private Animator animator = null;

    [SerializeField]
    private SkeletonMecanim skeletonMecanim = null;

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
                WalkForward();
            }
            else
            {
                Walkbackward();

            }
            int flipScale = thisScreenPoint.x > 0 ? 1 : -1;
            skeletonMecanim.transform.localScale = new Vector3(flipScale, 1, 1);
            
        }
        else
        {

            if (thisScreenPoint.x > 0)
            {
                WalkForward();
            }
            else
            {
                Walkbackward();
            }
            int flipScale = thisScreenPoint.y > 0 ? 1 : -1;
            skeletonMecanim.transform.localScale = new Vector3(flipScale, 1, 1);
        }

    }

    private void WalkForward()
    {
        if (!animator.GetBool("WalkForward"))
        {
            animator.SetTrigger("WalkForward");
            Debug.Log("Forward!");
        }

    }

    private void Walkbackward()
    {
        if (!animator.GetBool("WalkBackward"))
        {
            animator.SetTrigger("WalkBackward");
            Debug.Log("Backward!");
        }
            
    }

    private void Aim()
    {
        Vector2 input = new Vector2(Input.GetAxis("HorizontalRight"), Input.GetAxis("VerticalRight"));
        Vector3 aimVector = input;
        //
        if (input == Vector2.zero)
        {
            aimVector = Camera.main.WorldToScreenPoint(aimTransform.position);
            aimVector = Input.mousePosition - aimVector;

            //0 = Up, 1 =down
            //0.5 = sideview

            float animationRotation = aimVector.normalized.x * 0.5f;// - 0.5f;
            if (skeletonMecanim.transform.localScale.x == -1)
            {
                animationRotation = Mathf.Abs(animationRotation);
            }

            if (aimVector.normalized.y < 0)
            {
                animationRotation = 1f - animationRotation;
            }
            animationRotation = Mathf.Abs(animationRotation);
            animator.SetFloat("AimDirection", animationRotation);
            gunPoint.transform.localPosition = new Vector3(0, 1, 0);
        }
        else
        {
            gunPoint.transform.localPosition = new Vector3(1, 0, 0);
        }

        float angle = Mathf.Atan2(aimVector.y, aimVector.x) * Mathf.Rad2Deg;

        float normalizedAngle = Mathf.Abs(angle) / 180f;
        Debug.Log(angle + ":" + normalizedAngle);
        if (input != Vector2.zero)
        {
            animator.SetFloat("AimDirection", 1 - normalizedAngle);
        }
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
        ScreenShake.Instance.ShakeScreen();
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
            //spriteRenderer.color = Color.clear;
            skeletonMecanim.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.15f);
            skeletonMecanim.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.15f);
        }
        flickerCoroutine = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PowerUpPickable powerUpPickable = collision.gameObject.GetComponent<PowerUpPickable>();
        if (powerUpPickable != null)
        {

            ApplyBonus(powerUpPickable);
            Destroy(powerUpPickable.gameObject);
        }
    }

    private void ApplyBonus(PowerUpPickable powerUpPickable)
    {
        AudioSource.PlayClipAtPoint(powerUp, powerUpPickable.transform.position);

        //Do magic bonus shit
        playerStats.bulletCount += powerUpPickable.PowerUpSetting.PowerUp.bulletCount;
        playerStats.damage += powerUpPickable.PowerUpSetting.PowerUp.damage;
        playerStats.gunSpread += powerUpPickable.PowerUpSetting.PowerUp.gunSpread;
        playerStats.timeBetweenShotsMilis -= powerUpPickable.PowerUpSetting.PowerUp.timeBetweenShotsMilis;
        playerStats.bulletBounceCount += powerUpPickable.PowerUpSetting.PowerUp.bulletBounceCount;
        currentHealth += powerUpPickable.PowerUpSetting.PowerUp.maxHealth;
        playerStats.movementSpeed += powerUpPickable.PowerUpSetting.PowerUp.movementSpeed;
        playerStats.projectileSpeed += powerUpPickable.PowerUpSetting.PowerUp.projectileSpeed;
        playerStats.damageOverTime += powerUpPickable.PowerUpSetting.PowerUp.damageOverTime;
        playerStats.maxBulletRange += powerUpPickable.PowerUpSetting.PowerUp.maxBulletRange;

        if (playerStats.damage < 1)
            playerStats.damage = 1;
    }
}
