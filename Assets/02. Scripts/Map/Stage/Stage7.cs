using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage7 : StageBase
{
    [SerializeField] StageComposition drugDrugStage;

    [SerializeField] Transform cameraTrans;
    [SerializeField] Transform playerTrans;

    [SerializeField] GameObject obstacle;
    [SerializeField] GameObject cleanCollider;

    private bool isUseDrug = false;

    protected override void Awake()
    {
        base.Awake();
        StageIndex = 7;
        drugDrugStage.stageParent.SetActive(false);
    }

    public override Vector3 GetStartPosition() => isUseDrug switch
    {
        true => drugDrugStage.startPosition.position,
        false => isClean ? cleanStage.startPosition.position : drugStage.startPosition.position
    };

    public override float GetCameraPositionY() => isUseDrug switch
    {
        true => -40f,
        false => isClean? 0f : -20f
    };

    public override float GetMiddleCameraPositionX() => 312f;

    public override void SetStage(bool _isClean)
    {
        base.SetStage(_isClean);

        Base.Manager.Map.SetInvincible(false);
    }

    public override void UseDrug()
    {
        Base.Manager.Sound.PlaySFX("SFX_Stage7_Item");
        Base.Manager.PostProcessing.SetLensDistortion();

        if (isClean)
        {
            obstacle.SetActive(false);
            cleanCollider.SetActive(true);
        }
        else
        {
            isUseDrug = true;

            drugStage.stageParent.SetActive(false);
            drugDrugStage.stageParent.SetActive(true);

            cameraTrans.position = new Vector3(300f, -40f, -10f);
            playerTrans.position = drugDrugStage.startPosition.position;
        }
    }
}
