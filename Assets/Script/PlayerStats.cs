using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStats 
{
    public float maxHealth = 0;
    public float movementSpeed = 0;
    public float gunSpread = 0;
    public float projectileSpeed = 0;
    public float damage = 0;
    public float damageOverTime = 0;
    public float timeBetweenShotsMilis = 0;
    public int bulletCount = 0;
    public float maxBulletRange = 0;
    public int bulletBounceCount = 0;
    public int bulletPenitration = 0;
    // int count, float spread, float speed, float damage, float damageOverTimer, float timeBetweenShotsMilis)
}

/*
    public float maxHealth = 150;
    public float movementSpeed = 150;
    public float gunSpread = 0;
    public float projectileSpeed = 25;
    public float damage = 5;
    public float damageOverTime = 0;
    public float timeBetweenShotsMilis = 1000;
    public int bulletCount = 1;
    public float maxBulletRange = 25;
    public int bulletBounceCount = 0;
*/