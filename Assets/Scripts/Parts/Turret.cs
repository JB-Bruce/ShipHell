using UnityEngine;

public class Turret : ShipPart
{
    [SerializeField] protected GameObject bulletPrefab;

    [SerializeField] protected float bulletsPerSeconds;
    protected float shootTimer;

    [SerializeField] protected float shootForce;

    [SerializeField] protected Transform canonPart;

    [SerializeField] protected float damage;
    [SerializeField] protected int maxHits;

    protected void Update()
    {
        shootTimer -= Time.deltaTime;
    }

    protected virtual void Shoot(Vector2 dir, float angle)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform);
        bullet.transform.localPosition = Vector2.zero;

        bullet.GetComponent<PlayerBullet>().Init(dir, shootForce, angle, damage, maxHits);
    }
}
