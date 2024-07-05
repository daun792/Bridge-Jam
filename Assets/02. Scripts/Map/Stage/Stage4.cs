using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage4 : StageBase
{
    protected override void Awake()
    {
        base.Awake();
        StageIndex = 4;
    }

    public override void UseDrug()
    {
        Base.Manager.Map.ChangeJumpSpeed(0.5f);
    }
}
