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

    public override void SetStage(bool _isClean)
    {
        base.SetStage(_isClean);

        Base.Manager.Map.SetInvincible(false);
    }

    public override void ResetStage()
    {
        base.ResetStage();

        if (isUseDrug)
        {
            MoveCamera();
            MovePlayer();
        }
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

            MoveCamera();
            MovePlayer();
        }
    }

    private void MoveCamera()
    {
        cameraTrans.position = new Vector3(300, -40f, -10f);
    }

    private void MovePlayer()
    {
        playerTrans.position = drugDrugStage.startPosition.position;
    }
}
