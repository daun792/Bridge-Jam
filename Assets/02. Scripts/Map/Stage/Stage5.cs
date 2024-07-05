using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage5 : StageBase
{
    [SerializeField] Transform cleanDest;
    [SerializeField] Transform drugDest;

    public override void UseDrug()
    {
        Base.Manager.Sound.VolumeDownBGM();
        Base.Manager.Map.FlyToDestination(isClean ? cleanDest.position : drugDest.position);
        Base.Manager.PostProcessing.SetAnalogGlitch(true);
    }
}