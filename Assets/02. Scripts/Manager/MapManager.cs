using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MapManager : Manager
{
    [SerializeField] Transform cameraTrans;
    [SerializeField] Transform playerTrans;
    [SerializeField] Transform[] stageStartPosition;
    [SerializeField] RuntimeAnimatorController[] animators;
    [SerializeField] int timeLimit = 30;

    private int stageIndex = 1;
    private int drugCount = 0;
    private bool isUseDrug = false;
    private IEnumerator timeCheckRoutine;
    private CharacterBase player;

    private void Start()
    {
        cameraTrans.position = new Vector3(0f, 10f, -10f);
        cameraTrans.DOMoveY(0f, 1f).SetEase(Ease.OutCubic);
        player = playerTrans.GetComponent<CharacterBase>();
        timeCheckRoutine = CheckTime();
        StartCoroutine(timeCheckRoutine);
    }

    public void LoadNextStage()
    {
        if (stageIndex > 6) //TODO:
        {
            StartCoroutine(EndGame());

            return;
        }

        Base.Manager.UI.FadeInOut(InitStage);
    }

    public void ReloadStage()
    {
        if (stageIndex > 6) //TODO:
        {
            StartCoroutine(EndGame());
            return;
        }

        Base.Manager.UI.FadeInOut(ResetStage);
    }

    public void UseDrug()
    {
        isUseDrug = true;
    }

    public void ChangeState(FaceType _state)
    {
        player.SetAnimator(animators[(int)_state]);
    }

    public void ChangeSpeed(float _value)
    {
        player.MovementSpeed = _value;
    }

    public void ChangeJumpRange(float _value)
    {
        float inv = 1f / _value;
        player.JumpVelocity = inv;
        player.GravityScale = _value * _value;
    }

    private void InitStage()
    {
        if (isUseDrug)
        {
            SetDrugEffect();
        }

        MoveCamera();
        MovePlayer();

        stageIndex++;
        StopCoroutine(timeCheckRoutine);
        timeCheckRoutine = CheckTime();
        StartCoroutine(timeCheckRoutine);
        isUseDrug = false;
    }

    private void ResetStage()
    {
        stageIndex--;
        isUseDrug = false;
        InitStage();
    }

    private void MoveCamera()
    {
        var xPos = 30 * stageIndex;
        cameraTrans.position = new Vector3(xPos, 0f, -10f);
    }

    private void MovePlayer()
    {
        playerTrans.position = stageStartPosition[stageIndex].position;
    }

    private void SetDrugEffect()
    {
        ModifyPlayerSpeed(1f);
        drugCount++;
        if (drugCount == 8)
        {
            Base.Manager.UI.FaceChange(FaceType.Hallucinated);
        }
        else if (drugCount == 5)
        {
            Base.Manager.UI.FaceChange(FaceType.Delight);
        }
        switch (stageIndex)
        {
            case 1:
                Base.Manager.PostProcessing.SetSaturation(-30f);
                //Base.Manager.UI.FaceChange(FaceType.Delight);
                break;
            case 2:
                ModifyPlayerSpeed(0.8f);
                //Base.Manager.UI.FaceChange(FaceType.Hallucinated);
                break;
            case 3:
                timeLimit = 18;
                break;
            case 4:
                Base.Manager.PostProcessing.SetLensDistortion();
                break;
            case 6:
                Base.Manager.PostProcessing.SetFlashBack();
                break;
            case 7:
                StartCoroutine(GetBackToPreviousPlace(2f));
                StartCoroutine(GetBackToPreviousPlace(3f));
                StartCoroutine(GetBackToPreviousPlace(3.5f));
                StartCoroutine(GetBackToPreviousPlace(5f));
                break;
            case 8:
                RotateSight(true);
                break;
            case 9:
                StartCoroutine(WindowLotationLoop());
                break;

        }
    }

    private void ModifyPlayerSpeed(float _value)
    {
        ChangeSpeed(_value);
        ChangeJumpRange(_value);
    }

    private void RotateSight(bool _isRotated)
    {
        Camera.main.transform.rotation = Quaternion.Euler(0f, 0f, _isRotated ? 180f : 0f);
    }

    private IEnumerator EndGame()
    {
        Base.Manager.UI.FadeIn();

        yield return new WaitForSeconds(1f);

        Base.LoadScene(SceneName.Title);
    }

    private IEnumerator GetBackToPreviousPlace(float time)
    {
        yield return new WaitForSeconds(time - 0.5f);
        Vector2 pos = playerTrans.position;
        yield return new WaitForSeconds(0.5f);
        playerTrans.position = pos;
    }

    private IEnumerator WindowLotationLoop()
    {
        while (true) //change condition later
        {
            yield return new WaitForSeconds(1.5f);
            RotateSight(true);
            yield return new WaitForSeconds(0.5f);
            RotateSight(false);
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

        Base.Manager.UI.FadeInOut(ResetStage);
    }
    #endregion
}
