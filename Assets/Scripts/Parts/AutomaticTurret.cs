using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticTurret : Turret
{
    [SerializeField] float range;

    Enemy target;
    
    private new void Update()
    {
        if(isBroken) return;

        base.Update();

        

        Enemy closestTarget = enemiesManager.GetClosestEnemy(transform.position);
        if (closestTarget != null && Vector2.Distance(closestTarget.transform.position, transform.position) <= range)
        {
            target = closestTarget;
        }
        else
        {
            target = null;
            return;
        }


        Vector2 dir = target.transform.position - transform.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        canonPart.rotation = Quaternion.Euler(0, 0, angle);

        if (shootTimer <= 0)
        {
            shootTimer = bulletsPerSeconds;

            Shoot(dir.normalized, angle);
        }
    }
}
