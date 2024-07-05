using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2 : StageBase
{
    public override void UseDrug()
    {
        Base.Manager.Map.ChangeJumpSpeed(0.2f);
    }
}
