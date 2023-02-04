using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private new Rigidbody2D rigidbody2D = null;

    private float projectileSpeed = 15f;
    private float damage = 5f;
    private float damageOverTime = 0.25f;

    public void SetUp(float damage, float damageOverTime, float projectileSpeed)
    {
        this.damage = damage;
        this.damageOverTime = damageOverTime;
        this.projectileSpeed = projectileSpeed;
    }
    public void Shoot(Vector3 direction, Vector3 positionToSpawn)
    {
        transform.position = positionToSpawn;
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
        Destroy(this.gameObject);
        //rigidbody2D.velocity *= -1;
        //Debug.Log("Bouncy");
    }
}
