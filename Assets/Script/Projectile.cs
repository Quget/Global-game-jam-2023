using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private new Rigidbody2D rigidbody2D = null;

    private float projectileSpeed = 15f;
    private float damage = 5f;
    public float Damage => damage;

    private float damageOverTime = 0.25f;
    private float maxRange = 25;

    private Vector3 startPosition = Vector3.zero;

    public void SetUp(float damage, float damageOverTime, float projectileSpeed, float maxRange)
    {
        this.damage = damage;
        this.damageOverTime = damageOverTime;
        this.projectileSpeed = projectileSpeed;
        this.maxRange = maxRange;
    }


    private void LateUpdate()
    {
        float distance = Vector3.Distance(transform.position, startPosition);
        if (Mathf.Abs(distance) > maxRange)
        {
            Clear();
        }
    }

    public void Shoot(Vector3 direction, Vector3 positionToSpawn)
    {
        transform.position = positionToSpawn;
        startPosition = positionToSpawn;
        //rigidbody2D.AddForce(direction.normalized * projectileSpeed, ForceMode2D.Impulse);
        rigidbody2D.AddForce(direction.normalized * projectileSpeed, ForceMode2D.Impulse);
    }

    public void RotateRigidBody(float angle)
    {
        rigidbody2D.rotation = angle;
        //rigidbody2D.MoveRotation(angle);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Clear();
        //rigidbody2D.velocity *= -1;
        //Debug.Log("Bouncy");
    }

    private void Clear()
    {
        Destroy(this.gameObject);
    }
}
