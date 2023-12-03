using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    public LootEnum lootType;

    public bool isTargeted;

    private void Start()
    {
        LootManager.instance.AddLoot(this);
    }
}

[System.Serializable]
public enum LootEnum
{
    Basic = 10,
    Advanced = 8,
    Special = 2,
    Epic = 0
}
