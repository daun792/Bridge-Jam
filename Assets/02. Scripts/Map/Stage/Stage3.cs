using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3 : StageBase
{
    [SerializeField] GameObject cleanRainbow;
    [SerializeField] GameObject drugRainbow;

    public override void UseDrug()
    {
        Base.Manager.Sound.VolumeDownBGM();
        Base.Manager.Sound.PlaySFX("SFX_Stage3_Item");

        if (isClean)
        {
            cleanRainbow.SetActive(true);
        }
        else
        {
            drugRainbow.SetActive(true);
        }
    }
}
