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

    private List<StageBase> stages;
    private StageBase currStage;

    [SerializeField] private int stageIndex = 0;
    private int debuffIndex = 0;

    private float timeRate = 1f;
    private int drugCount = 0;
    public bool IsClean { get; private set; }

    private IEnumerator windowInvert;
    private IEnumerator backToPreviousPlace;
    private IEnumerator timeCheckRoutine;
    private CharacterBase player;

    private Camera cam;
    private Matrix4x4 originMatrix;


    private void Start()
    {
        IsClean = true;

        stages = GetComponentsInChildren<StageBase>().ToList();
        player = playerTrans.GetComponent<CharacterBase>();
        cam = cameraTrans.GetComponent<Camera>();
        originMatrix = cam.projectionMatrix;

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
        stageIndex++;

        Base.Manager.Sound.StopBGM();
        
        Base.Manager.UI.FadeInOut(SetCurrStage);

        SetDrugEffect();
    }

    public void SetCurrStage()
    {
        if (currStage != null)
            Destroy(currStage.gameObject);
        
        currStage = stages.Find(x => x.StageIndex == stageIndex);
        currStage.SetStage(IsClean);

        InitStage();
    }

    private void InitStage()
    {
        player.SetVelocityZero();
        MoveCamera();
        MovePlayer();

        if (timeCheckRoutine != null)
            StopCoroutine(timeCheckRoutine);
        timeCheckRoutine = CheckTime();
        StartCoroutine(timeCheckRoutine);
    }

    public IEnumerator PlayerDead()
    {
        Base.Manager.Sound.PlaySFX("SFX_PlayerDead");
        player.Die();

        yield return new WaitForSeconds(0.5f);

        Base.Manager.UI.FadeInOut(ReloadStage);

        player.Respawn();
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
        SetPlayerToSpace(false);
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
                SetWindowLotation();
                if (debuffIndex + 1 == stageIndex) break;
                goto case 9;

            case 9:
                InvertCamera();
                SetTimeBacking();
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
                timeRate = 0.8f;
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
        else if(drugCount >= 5)
        {
            Base.Manager.Sound.PlaySFX("SFX_GetItem");
            Base.Manager.UI.FaceChange(FaceType.Delight);
            ChangeState(FaceType.Delight);
        }
        else
        {
            Base.Manager.Sound.PlaySFX("SFX_GetItem");
        }

        Base.Manager.Sound.PitchBGM(0.9f);

        drugCount++;
        IsClean = false;
        currStage.UseDrug();
    }

    public void MoveCameraToMiddle()
    {
        var xPos = currStage.GetMiddleCameraPositionX();
        cameraTrans.DOMoveX(xPos, 1f).SetEase(Ease.Linear);
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

    public void SetInvincible(bool _isInvincible)
    {
        player.Invincible = _isInvincible;
        if (_isInvincible)
        {
            Base.Manager.PostProcessing.SetFlashBack();
            player.SetFlashBack();
        }
    }

    public void FlyToDestination(Vector3 _dest)
    {
        player.Fly(_dest);
    }

    private void InvertCamera() 
    {
        Matrix4x4 mat = originMatrix;
        mat *= Matrix4x4.Scale(new Vector3(1, -1, 1));

        cam.projectionMatrix = mat;
    }

    public void ModifyPlayerSpeed(float _value)
    {
        ChangeSpeed(_value);
    }

    private void SetTimeBacking()
    {
        if (backToPreviousPlace != null)
            StopCoroutine(backToPreviousPlace);
        backToPreviousPlace = GetBackToPreviousPlace();
        StartCoroutine(backToPreviousPlace);
    }

    public void StopTimeBacking()
    {
        if (backToPreviousPlace != null)
            StopCoroutine(backToPreviousPlace);
    }

    public void SetPlayerToSpace(bool _isSpace)
    {
        player.IsSpace = _isSpace;
        ChangeJumpSpeed(_isSpace ? 0.5f : 1f);
    }

    private IEnumerator GetBackToPreviousPlace()
    {
        float interval = Random.Range(2f, 5f);

        while (true)
        {
            yield return new WaitForSeconds(interval - 0.5f);

            Vector2 pos = playerTrans.position;

            yield return new WaitForSeconds(0.5f);

            playerTrans.position = pos;
            interval = Random.Range(2f, 5f);
        }
    }

    private void SetWindowLotation()
    {
        if (windowInvert != null)
            StopCoroutine(windowInvert);
        windowInvert = WindowLotationLoop();
        StartCoroutine(windowInvert);
    }

    public void StopWindowLotation()
    {
        if (windowInvert != null)
            StopCoroutine(windowInvert);
    }

    private IEnumerator WindowLotationLoop()
    {
        Matrix4x4 mat = originMatrix;
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

    public void StopTimer()
    {
        if (timeCheckRoutine != null) 
            StopCoroutine(timeCheckRoutine);
    }

    #region Time
    private IEnumerator CheckTime()
    {
        float time = currStage.StageTime * timeRate;
        float currTime = time;
        while (currTime > 0f)
        {
            currTime -= Time.deltaTime;
            Base.Manager.UI.SetTime(currTime / time);
            yield return null;
        }

        Base.Manager.UI.FadeInOut(ReloadStage);
    }
    #endregion
}
