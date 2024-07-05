using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MapManager : Manager
{
    [SerializeField] Transform cameraTrans;
    [SerializeField] Transform playerTrans;
    [SerializeField] GameObject blurParent;

    [SerializeField] RuntimeAnimatorController[] animators;
    [SerializeField] int timeLimit = 30;

    private List<StageBase> stages;
    public StageBase currStage;

    [SerializeField] private int stageIndex = 0;
    private int debuffIndex = 0;

    private int drugCount = 0;
    public bool IsClean { get; private set; }

    private IEnumerator backToPreviousPlace;
    private IEnumerator timeCheckRoutine;
    private CharacterBase player;

    private Camera cam;

    private void Start()
    {
        IsClean = true;

        stages = GetComponentsInChildren<StageBase>().ToList();
        player = playerTrans.GetComponent<CharacterBase>();
        cam = cameraTrans.GetComponent<Camera>();

        Sequence sequence = DOTween.Sequence();
        sequence.OnStart(() =>
            {
                stageIndex++;
                currStage = stages.Find(x => x.StageIndex == stageIndex);
                currStage.SetStage(IsClean);

                cameraTrans.position = new Vector3(0f, 20f, -10f);
            })
            .Append(cameraTrans.DOMoveY(0f, 1f).SetEase(Ease.Linear))
            .OnComplete(() =>
            {
                Base.Manager.UI.InitPPCanvas();
                InitStage();
                Base.Manager.Sound.PlayBGM("BGM_Clean");
            });
    }

    public void LoadStage()
    {
        Base.Manager.Sound.StopBGM();

        if (currStage != null)
            Destroy(currStage.gameObject);
        stageIndex++;
        currStage = stages.Find(x => x.StageIndex == stageIndex);
        currStage.SetStage(IsClean);

        Base.Manager.UI.FadeInOut(InitStage);

        SetDrugEffect();
    }

    private void InitStage()
    {
        MoveCamera();
        MovePlayer();

        if (timeCheckRoutine != null)
            StopCoroutine(timeCheckRoutine);
        timeCheckRoutine = CheckTime();
        StartCoroutine(timeCheckRoutine);
    }

    public void PlayerDead()
    {
        Base.Manager.Sound.PlaySFX("SFX_PlayerDead");

        Base.Manager.UI.FadeInOut(ReloadStage);
    }

    private void ReloadStage()
    {
        InitStage();
        currStage.ResetStage();
    }

    

    private void MoveCamera()
    {
        var xPos = 50 * stageIndex - 50;
        var yPos = currStage.GetCameraPositionY();
        cameraTrans.position = new Vector3(xPos, yPos, -10f);
    }

    private void MovePlayer()
    {
        playerTrans.position = currStage.GetStartPosition();
    }

    private void SetDrugEffect()
    {
        if (IsClean)
        {
            Base.Manager.Sound.ResumeBGM();
            return;
        }

        if (Base.Manager.Sound.GetPlayingBGM() == "CleanBGM")
        {
            Base.Manager.Sound.PlayBGM("BGM_Drug");
        }
        else if (Base.Manager.Sound.GetPlayingBGM() == "DrugBGM") 
        {
            Base.Manager.Sound.ResumeBGM();
        }

        switch (stageIndex)
        {
            case 10:
                StartCoroutine(WindowLotationLoop());
                if (debuffIndex + 1 == stageIndex) break;
                goto case 9;

            case 9:
                InvertCamera();
                if (debuffIndex + 1 == stageIndex) break;
                goto case 8;

            case 8:
                SetTimeBacking();
                if (debuffIndex + 1 == stageIndex) break;
                goto case 7;

            case 7:
                Base.Manager.PostProcessing.SetFlashBack();
                if (debuffIndex + 1 == stageIndex) break;
                goto case 6;

            case 6:
            case 5:
                blurParent.SetActive(true);
                if (debuffIndex + 1 == stageIndex) break;
                goto case 4;

            case 4:
                timeLimit = 18;
                if (debuffIndex + 1 == stageIndex) break;
                goto case 3;

            case 3:
                ModifyPlayerSpeed(0.8f);
                if (debuffIndex + 1 == stageIndex) break;
                goto case 2;

            case 2:
                Base.Manager.PostProcessing.SetSaturation(-30f);
                break;
        }

        debuffIndex = stageIndex;
    }

    public void UseDrug()
    {
        if (drugCount >= 8)
        {
            Base.Manager.Sound.PlaySFX("SFX_GetItem_Sick");
            Base.Manager.UI.FaceChange(FaceType.Hallucinated);
            ChangeState(FaceType.Hallucinated);
        }
        else
        {
            Base.Manager.Sound.PlaySFX("SFX_GetItem");
            Base.Manager.UI.FaceChange(FaceType.Delight);
            ChangeState(FaceType.Delight);
        }

        Base.Manager.Sound.PitchBGM();

        drugCount++;
        IsClean = false;
        currStage.UseDrug();
    }

    public void ChangeState(FaceType _state)
    {
        player.SetAnimator(animators[(int)_state]);
    }

    public void ChangeSpeed(float _value)
    {
        player.MovementSpeed = _value;
    }

    public void ChangeJumpSpeed(float _value)
    {
        float inv = 1f / _value;
        player.JumpVelocity = inv;
        player.GravityScale = _value * _value;
    }

    public void ChangeJumpHeight(float _value)
    {
        player.JumpVelocity = _value;
    }

    public void SetInvincible(bool _isInvincible)
    {
        player.Invincible = _isInvincible;
    }

    public void FlyToDestination(Vector3 _dest)
    {
        player.Fly(_dest);
    }

    private void InvertCamera() 
    {
        Matrix4x4 mat = cam.projectionMatrix;
        mat *= Matrix4x4.Scale(new Vector3(1, -1, 1));

        cam.projectionMatrix = mat;
    }






    public void ModifyPlayerSpeed(float _value)
    {
        ChangeSpeed(_value);
        ChangeJumpSpeed(_value);
    }

    private void SetTimeBacking()
    {
        backToPreviousPlace = GetBackToPreviousPlace();
        StartCoroutine(backToPreviousPlace);
    }

    public void StopTimeBacking()
    {
        StopCoroutine(backToPreviousPlace);
    }

    private IEnumerator GetBackToPreviousPlace()
    {
        float interval = Random.Range(3f, 10f);

        while (true)
        {
            yield return new WaitForSeconds(interval - 0.5f);

            Vector2 pos = playerTrans.position;

            yield return new WaitForSeconds(0.5f);

            playerTrans.position = pos;
            interval = Random.Range(2f, 7f);
        }
    }

    private IEnumerator WindowLotationLoop()
    {
        Matrix4x4 mat = cam.projectionMatrix;
        var invertXMat = mat * Matrix4x4.Scale(new Vector3(-1, -1, 1));
        var invertYMat = mat * Matrix4x4.Scale(new Vector3(1, -1, 1));

        while (true) //change condition later
        {
            yield return new WaitForSeconds(1.5f);

            cam.projectionMatrix = invertXMat;

            yield return new WaitForSeconds(0.5f);

            cam.projectionMatrix = invertYMat;
        }
    }

    #region Time
    private IEnumerator CheckTime()
    {
        float time = timeLimit;
        while (time > 0f)
        {
            time -= Time.deltaTime;
            Base.Manager.UI.SetTime(time / timeLimit);
            yield return null;
        }

        Base.Manager.UI.FadeInOut(ReloadStage);
    }
    #endregion
}
