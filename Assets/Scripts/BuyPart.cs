using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class BuyPart : MonoBehaviour
{
    [SerializeField] GameObject shipPartPrefab;

    [SerializeField] LootTextStruct[] texts;

    private void Start()
    {
        SetPrices(shipPartPrefab.GetComponent<ShipBuildPart>().prices);
    }

    private void SetPrices(List<Price> prices)
    {
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].lootAmount = 0;

            foreach (var item in prices)
            {
                if (texts[i].lootEnum == item.loot)
                {
                    texts[i].lootAmount = item.amount;
                    break;
                }
            }

            if (texts[i].lootAmount == 0)
                texts[i].text.transform.parent.parent.gameObject.SetActive(false);

            texts[i].text.text = texts[i].lootAmount.ToString();
        }
    }

    private void OnMouseDown()
    {
        if(ShipConstruction.instance.TrySpendRessources(shipPartPrefab.GetComponent<ShipBuildPart>().prices))
            ShipConstruction.instance.SetDraggedPart(Instantiate(shipPartPrefab, transform.parent));
    }
}
