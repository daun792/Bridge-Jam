using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2 : StageBase
{
    protected override void Awake()
    {
        base.Awake();
        StageIndex = 2;
    }

    public override void UseDrug()
    {
        Base.Manager.Map.ChangeJumpSpeed(0.2f);
    }
}
