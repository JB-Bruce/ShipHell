using System.Collections.Generic;
using UnityEngine;

public class Harvester : ShipPart
{
    [SerializeField] List<Drone> drones = new();

    [SerializeField] float checkTime;

    protected override void Start()
    {
        base.Start();

        foreach (var item in drones)
        {
            item.transform.position = transform.position;
        }

        InvokeRepeating("CheckForLoot", 0, checkTime);
    }

    private void CheckForLoot()
    {
        foreach (var drone in drones)
        {
            if (drone.available)
            {
                Loot target = LootManager.instance.GetClosestNonTargetedLoot(transform.position);

                if (target != null)
                    drone.SetTarget(target);

                return;
            }
        }
    }
}
