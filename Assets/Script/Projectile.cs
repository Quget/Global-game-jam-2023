using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private new Rigidbody2D rigidbody2D = null;

    [SerializeField]
    private float projectileSpeed = 15f;


    public void Shoot(Vector3 direction, Vector3 positionToSpawn)
    {
        transform.position = positionToSpawn;
        rigidbody2D.AddForce(direction.normalized * projectileSpeed, ForceMode2D.Impulse);
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
        //rigidbody2D.velocity *= -1;
        //Debug.Log("Bouncy");
    }
}
