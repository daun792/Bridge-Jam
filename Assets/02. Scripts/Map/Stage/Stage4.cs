using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage4 : StageBase
{
    [SerializeField] DrowningTile[] cleanDrowningTiles;
    [SerializeField] DrowningTile[] drugDrowningTiles;

    protected override void Awake()
    {
        base.Awake();
        StageIndex = 4;
    }

    public override void UseDrug()
    {
        Base.Manager.Map.ChangeJumpSpeed(0.5f);
        if (isClean)
        {
            foreach (var tile in cleanDrowningTiles)
            {
                tile.IsDrwoning = true;
            }
        }
        else
        {
            foreach (var tile in drugDrowningTiles)
            {
                tile.IsDrwoning = true;
            }
        }
    }

    public override void ResetStage()
    {
        base.ResetStage();
        foreach (var tile in cleanDrowningTiles)
        {
            tile.IsDrwoning = false;
        }
        foreach (var tile in drugDrowningTiles)
        {
            tile.IsDrwoning = false;
        } 
    }
}
