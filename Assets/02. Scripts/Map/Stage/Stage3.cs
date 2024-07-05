using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3 : StageBase
{
    [SerializeField] GameObject cleanRainbow;
    [SerializeField] GameObject drugRainbow;

    protected override void Awake()
    {
        base.Awake();
        StageIndex = 3;
    }

    public override void UseDrug()
    {
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
