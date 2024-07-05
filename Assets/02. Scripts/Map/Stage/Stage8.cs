using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage8 : StageBase
{
    [SerializeField] Transform cleanFallObstacleParent;
    [SerializeField] Transform drugFallObstacleParent;

    public override float GetMiddleCameraPositionX() => 362f;

    public override void UseDrug()
    {
        Base.Manager.Map.StopTimeBacking();
        Base.Manager.Map.ModifyPlayerSpeed(1f);

        FallObstacle[] obstacles;

        if (isClean)
        {
            obstacles = cleanFallObstacleParent.GetComponentsInChildren<FallObstacle>(true);
        }
        else
        {
            obstacles = drugFallObstacleParent.GetComponentsInChildren<FallObstacle>(true);
        }

        foreach (var obstacle in obstacles)
        {
            obstacle.SetItemEffect();
        }
    }
}
