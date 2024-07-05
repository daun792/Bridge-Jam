using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage9 : StageBase
{
    public override float GetCameraPositionY() => 0f;

    public override float GetMiddleCameraPositionX() => 412f;

    public override void UseDrug()
    {
        Base.Manager.Map.SetPlayerToSpace(true);
    }
}
