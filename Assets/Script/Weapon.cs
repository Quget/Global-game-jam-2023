using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private Projectile projectile;

    //[SerializeField]
    private float timeBetweenShotsMilis = 1;

    private bool canShoot = true;

    public void Shoot(Vector3 direction, Vector3 positionToSpawn, int count, float spread, float speed, float damage, float damageOverTimer, float timeBetweenShotsMilis,float maxRange,int maxBouncyCount)
    {
        if (canShoot)
        {
            canShoot = false;
            this.timeBetweenShotsMilis = timeBetweenShotsMilis;
            UberShot(direction, positionToSpawn, count, spread,speed,damage,damageOverTimer,maxRange, maxBouncyCount);
            StartCoroutine(ShootTimer());
        }
    }

    private void SingleShot(Vector3 direction, Vector3 positionToSpawn, float speed, float damage, float damageOverTimer, float maxRange, int maxBouncyCount)
    {
        Projectile shotProjectile = GameObject.Instantiate(projectile);
        shotProjectile.SetUp(damage, damageOverTimer, speed, maxRange,maxBouncyCount);
        shotProjectile.Shoot(direction, positionToSpawn);
    }

    //ToDo
    private void UberShot(Vector3 direction, Vector3 positionToSpawn, int count, float spread,float speed, float damage, float damageOverTimer, float maxRange, int maxBouncyCount)
    {
        spread = spread * 0.5f;
        for (int i = 0; i < count; i++)
        {
            Quaternion angle =  Quaternion.Euler(0, 0, Random.Range(-spread, spread));
            Vector3 newDirection = angle * direction;
            SingleShot(newDirection, positionToSpawn,speed,damage,damageOverTimer,maxRange,maxBouncyCount);
        }
    }

    private IEnumerator ShootTimer()
    {
        yield return new WaitForSeconds(timeBetweenShotsMilis * 0.001f);
        canShoot = true;
    }
}
