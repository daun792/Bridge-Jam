using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage6 : StageBase
{
    public override void UseDrug()
    {
        Base.Manager.Sound.PlaySFX("SFX_Stage6_Item");
        Base.Manager.Map.SetInvincible(true);
        Base.Manager.Map.ChangeSpeed(3f);
    }
}
