using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1 : StageBase
{
    [SerializeField] GameObject obstacle;

    protected override void Awake()
    {
        base.Awake();
        StageIndex = 1;
    }

    public override void UseDrug()
    {
        obstacle.SetActive(false);
    }
}
