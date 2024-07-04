using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEditor.ShaderGraph.Internal;

public class UIManager : Manager
{
    [SerializeField] Image blackBlur;
    [SerializeField] Image faceImg;
    [SerializeField] Slider tiemSlider;
    [SerializeField] Image[] itemImgs;
    [SerializeField] Sprite[] faceSprites;
    [SerializeField] Sprite[] itemSprites;

    protected override void Awake()
    {
        base.Awake();

        //TODO: Additional Initialize
    }

    private void Update() 
    {
        InputKey();
    }

    private void InputKey() 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //TODO: Go to Title Scene?
        }

        //TODO: Additional Code?? ex) active option panel
    }

    #region Fade In / Out
    public void FadeIn(Action _endEvent = null)
    {
        Base.Manager.Sound.StopBGM();

        if (blackBlur.color.a == 1f)
        {
            _endEvent?.Invoke();
            return;
        }

        blackBlur.gameObject.SetActive(true);

        blackBlur.DOKill();
        blackBlur.DOFade(1f, 0.5f).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                _endEvent?.Invoke();
            });
    }

    public void FadeOut(Action _endEvent = null)
    {
        if (blackBlur.color.a == 0f)
        {
            _endEvent?.Invoke();
            return;
        }

        blackBlur.DOFade(0f, 1f).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                _endEvent?.Invoke();
                blackBlur.gameObject.SetActive(false);
            });
    }

    public void FadeInOut(Action _midEvent = null)
    {
        Base.Manager.Sound.StopBGM();

        if (blackBlur.color.a == 1f)
        {
            _midEvent?.Invoke();
            FadeOut();
            return;
        }

        blackBlur.gameObject.SetActive(true);

        blackBlur.DOKill();

        Sequence sequence = DOTween.Sequence();
        sequence.Append(blackBlur.DOFade(1f, 0.2f).SetEase(Ease.Linear))
            .AppendInterval(0.3f)
            .OnComplete(() =>
            {
                _midEvent?.Invoke();
                FadeOut();
            });
    }
    #endregion

    #region Face Change
    public void FaceChange(FaceType _type)
    {
        faceImg.sprite = faceSprites[(int)_type];
        Base.Manager.Map.ChangeState(_type);
    }
    #endregion

    #region Time
    public void SetTime(float _time)
    { 
        _time = Mathf.Clamp(_time, 0f, 1f);
        tiemSlider.value = _time;
    }
    #endregion

    #region Items
    public void ActiveItem(int _index)
    {
        itemImgs[_index].sprite = itemSprites[_index];
    }
    #endregion
}
