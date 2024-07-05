using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage9 : StageBase
{
    public override float GetCameraPositionY() => 0f;

    public override float GetMiddleCameraPositionX() => 412f;

    public override void SetStage(bool _isClean)
    {
        base.SetStage(_isClean);

        Base.Manager.Sound.PlayBGM(_isClean ? "BGM_Clean" : "BGM_Drug");
    }

    public override void UseDrug()
    {
        Base.Manager.Sound.SpatialBlendBGM();
        Base.Manager.Map.SetPlayerToSpace(true);
    }
}
