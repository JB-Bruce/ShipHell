using System.Collections.Generic;
using UnityEngine;

public class PartsManager : MonoBehaviour
{
    public List<ShipPart> partList;

    

    public static PartsManager instance;

    private void Awake()
    {
        instance = this;
    }

    public ShipPart GetClosestPart(Vector2 pos)
    {
        return GetClosestPartFromList(pos, partList);
    }

    public ShipPart GetClosestPartFromList(Vector2 pos, List<ShipPart> newPartList)
    {
        ShipPart closestPart = null;

        foreach (var part in newPartList)
        {
            if(closestPart == null || Vector2.Distance(part.transform.position, pos) < Vector2.Distance(closestPart.transform.position, pos))
            {
                closestPart = part;
            }
        }

        return closestPart;
    }



    public ShipPart GetClosestNonDestroyPart(Vector2 pos)
    {
        return GetClosestPartFromList(pos, GetNonDestroyedParts());
    }

    public List<ShipPart> GetNonDestroyedParts()
    {
        List<ShipPart> newPartList = new();

        foreach (var part in partList)
        {
            if (!part.isBroken)
                newPartList.Add(part);
        }

        return newPartList;
    }
}
