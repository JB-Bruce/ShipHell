using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RangedEnemy : Enemy
{
    [SerializeField] float range;

    [SerializeField] protected GameObject bulletPrefab;

    [SerializeField] protected float bulletsPerSeconds;
    float shootTimer;

    [SerializeField] protected float shootForce;

    [SerializeField] protected float damage;
    [SerializeField] protected int maxHits;

    [SerializeField] protected float unaccuracy;

    protected new void Start()
    {
        base.Start();
        InvokeRepeating("LookingForEnemies", 1, Random.Range(.1f, 3f));
    }

    private void LookingForEnemies()
    {
        if (target == null)
            return;

        if (Vector2.Distance(target.transform.position, transform.position) <= range)
        {
            canMove = false;
            
        }
    }

    private new void Update()
    {
        base.Update();

        shootTimer -= Time.deltaTime;

        if (!canMove) 
        {
            if (shootTimer <= 0)
            {
                shootTimer = bulletsPerSeconds;
                Shoot();
            }
        }
    }

    private void Shoot()
    {
        if (target == null)
            return;

        Vector2 dir = target.transform.position - transform.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        angle += Random.Range(-unaccuracy, unaccuracy);

        var newAngle = angle * Mathf.Deg2Rad;

        Vector2 newDir = new Vector2((float)Mathf.Cos(newAngle), (float)Mathf.Sin(newAngle));

        GameObject bullet = Instantiate(bulletPrefab, bulletsParent);
        bullet.transform.position = transform.position;

        bullet.GetComponent<EnemyBullet>().Init(newDir.normalized, shootForce, angle, damage, maxHits);
    }


    
}
