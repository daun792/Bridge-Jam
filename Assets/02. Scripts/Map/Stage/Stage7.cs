using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage7 : StageBase
{
    [SerializeField] Transform itemStartPosition;
    [SerializeField] Transform cameraTrans;
    [SerializeField] Transform playerTrans;
    [SerializeField] GameObject cleanCollider;
    [SerializeField] GameObject drugCollider;

    private bool isUseDrug = false;

    
    void Start()
    {
        StageIndex = 7;
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
        Base.Manager.PostProcessing.SetLensDistortion();

        if (isClean)
        {
            cleanCollider.SetActive(true);
        }
        else
        {
            isUseDrug = true;

            drugCollider.SetActive(true);

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
        playerTrans.position = itemStartPosition.position;
    }
}
