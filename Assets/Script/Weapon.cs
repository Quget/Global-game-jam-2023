using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private Projectile projectile;

    [SerializeField]
    private float timeBetweenShotsMilis = 1;

    private bool canShoot = true;

    public void Shoot(Vector3 direction, Vector3 positionToSpawn)
    {
        if (canShoot)
        {
            canShoot = false;
            SingleShot(direction, positionToSpawn);
            SpreadShot(direction, positionToSpawn,5);
            StartCoroutine(ShootTimer());
        }
    }

    private void SingleShot(Vector3 direction, Vector3 positionToSpawn)
    {
        Projectile shotProjectile = GameObject.Instantiate(projectile);
        shotProjectile.Shoot(direction, positionToSpawn);
    }

    private void SpreadShot(Vector3 direction, Vector3 positionToSpawn, int count)
    {
        //direction = direction * (1f / count);
        for (int i = 1; i < count + 1; i++)
        {
            SingleShot(direction * ((float)i / (float)count), positionToSpawn);
        }
    }

    private IEnumerator ShootTimer()
    {
        yield return new WaitForSeconds(timeBetweenShotsMilis * 0.001f);
        canShoot = true;
    }
}
