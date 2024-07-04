using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1 : StageBase
{
    [SerializeField] GameObject obstacle;

    void Start()
    {
        StageIndex = 1;
    }

    public override void UseDrug()
    {
        obstacle.SetActive(false);
    }
}
