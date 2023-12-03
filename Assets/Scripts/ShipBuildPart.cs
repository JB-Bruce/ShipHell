using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBuildPart : MonoBehaviour
{
    public GameObject previewObject;

    

    public List<Price> prices;

    void Start()
    {
        previewObject.SetActive(false);
        ShipConstruction.instance.shipParts.Add(GetComponent<ShipPart>());
    }
}

[System.Serializable]
public struct Price
{
    public LootEnum loot;
    public int amount;
}
