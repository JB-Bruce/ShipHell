using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPart : MonoBehaviour
{
    protected float baseLife;
    [SerializeField] protected float partLife;

    [SerializeField] protected GameObject brokeEffect;

    protected EnemiesManager enemiesManager;

    public bool isBroken { get; protected set; }

    protected virtual void Start()
    {
        baseLife = partLife;
        enemiesManager = EnemiesManager.instance;
    }

    public virtual void TakeDamage(float damage)
    {
        if (isBroken)
            return;

        partLife -= damage;
        if(partLife <= 0)
        {
            Instantiate(brokeEffect, transform.position, Quaternion.identity);
            Broke();
        }
            
    }

    public virtual void Broke()
    {
        isBroken = true;
        gameObject.SetActive(false);
    }

    public virtual void Heal(float amount)
    {
        partLife = Mathf.Clamp(partLife + amount, 0f, baseLife);
    }
}
