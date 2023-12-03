using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedStation : ShipPart
{
    [SerializeField] protected float range;

    [SerializeField] protected float healTime;
    [SerializeField] protected float healAmount;

    protected PartsManager pm;

    protected override void Start()
    {
        base.Start();

        pm = PartsManager.instance;

        InvokeRepeating("Heal", 0, healTime);
    }

    protected virtual void Heal()
    {
        List<ShipPart> shipList = pm.GetNonDestroyedParts();

        foreach (ShipPart shipPart in shipList)
        {
            if(Vector2.Distance(shipPart.transform.position, transform.position) <= range)
            {
                shipPart.Heal(healAmount);
            }
        }
    }
}
