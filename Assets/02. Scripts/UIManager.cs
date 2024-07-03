using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : Manager
{
    [SerializeField] Image blackBlur;

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
        blackBlur.DOFade(1f, 0.5f).SetEase(Ease.Linear)
           .OnComplete(() =>
           {
               _midEvent?.Invoke();
               FadeOut();
           });
    }
    #endregion
}
