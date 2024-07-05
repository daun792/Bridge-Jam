using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage5 : StageBase
{
    [SerializeField] Transform cleanDest;
    [SerializeField] Transform drugDest;

    protected override void Awake()
    {
        base.Awake();
        StageIndex = 5;
    }

    public override void UseDrug()
    {
        Base.Manager.Map.FlyToDestination(isClean ? cleanDest.position : drugDest.position);
    }
}