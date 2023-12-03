using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Enemy>(out Enemy enemy))
        {
            if (!enemy.TakeDamage(damage))
                return;
            pierceAmount++;

            if (pierceAmount >= maxHits)
            {
                Instantiate(impactEffect, collision.ClosestPoint(transform.position), Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
