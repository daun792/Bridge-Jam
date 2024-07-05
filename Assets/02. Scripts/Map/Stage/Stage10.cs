using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage10 : StageBase
{
    protected override void Awake()
    {
        base.Awake();
        StageIndex = 10;
    }

    public override float GetCameraPositionY() => 0f;

    public override void UseDrug()
    {

    }
}
