using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage9 : StageBase
{
    protected override void Awake()
    {
        base.Awake();
        StageIndex = 9;
    }

    public override float GetCameraPositionY() => 0f;

    public override float GetMiddleCameraPositionX() => 412f;

    public override void UseDrug()
    {
        Base.Manager.Map.SetPlayerToSpace(true);
    }
}
