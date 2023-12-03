using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : ShipPart
{
    public override void Broke()
    {
        GameManager.instance.EndGame();

        base.Broke();
    }
}
