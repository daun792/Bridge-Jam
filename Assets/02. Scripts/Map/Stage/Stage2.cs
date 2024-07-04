using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2 : StageBase
{
    void Start()
    {
        StageIndex = 2;
    }

    public override void UseDrug()
    {
        Base.Manager.Map.ChangeJumpRange(5f);
    }
}
