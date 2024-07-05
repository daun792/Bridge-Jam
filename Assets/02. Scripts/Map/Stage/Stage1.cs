using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1 : StageBase
{
    [SerializeField] GameObject obstacle;

    public override void UseDrug()
    {
        obstacle.SetActive(false);
    }
}
