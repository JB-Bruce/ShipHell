using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    List<Loot> lootOnMap = new();

    public static LootManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void AddLoot(Loot loot)
    {
        lootOnMap.Add(loot);
    }

    public Loot GetClosestNonTargetedLoot(Vector2 pos)
    {
        List<Loot> newList = new();

        foreach (Loot item in lootOnMap)
        {
            if(!item.isTargeted)
                newList.Add(item);
        }

        return GameManager.GetClosestFromList<Loot>(pos, newList);
    }

    public Loot GetClosestLoot(Vector2 pos)
    {
        return GameManager.GetClosestFromList<Loot>(pos, lootOnMap);
    }
}
