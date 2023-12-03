using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected float damage;

    protected int pierceAmount = 0;
    protected int maxHits;

    [SerializeField] protected GameObject impactEffect;

    [SerializeField] protected Rigidbody2D rb;

    public void Init(Vector2 dir, float force, float angle, float damage, int maxHits)
    {
        rb.AddForce(dir * force, ForceMode2D.Impulse);
        rb.rotation = angle;

        this.damage = damage;
        this.maxHits = maxHits;

        Invoke("StopBullet", 5);
    }

    private void Update()
    {
        if (transform.position.magnitude > 15f)
            Destroy(gameObject);
    }

    private void StopBullet()
    {
        Destroy(gameObject);
    }
}
