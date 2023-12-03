using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<ShipPart>(out ShipPart part))
        {
            if (part.isBroken)
                return;

            part.TakeDamage(damage);
            pierceAmount++;

            if (pierceAmount >= maxHits)
            {
                Instantiate(impactEffect, collision.ClosestPoint(transform.position), Quaternion.identity);
                Destroy(gameObject);
            }
                
        }
    }

    
}
