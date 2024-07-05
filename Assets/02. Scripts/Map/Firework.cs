using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Firework : MonoBehaviour
{
    [SerializeField] GameObject firework;
    [SerializeField] GameObject tail;

    private float firePositionY = -8f;
    [SerializeField] float explosionPositionY;

    [SerializeField] float startDelay = 1f;

    private void OnEnable()
    {
        PlayFirework();
    }

    private void OnDisable()
    {
        transform.DOKill();
    }

    private void PlayFirework()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.OnStart(() =>
            {
                firework.SetActive(false);
                tail.SetActive(true);
            })
            .Append(transform.DOMoveY(firePositionY, 0f))
            .AppendInterval(startDelay)
            .Append(transform.DOMoveY(explosionPositionY, 1f).SetEase(Ease.OutCubic))
            .AppendCallback(() =>
            {
                firework.SetActive(true);
                tail.SetActive(false);
            })
            .AppendInterval(3.1f)
            .OnComplete(() =>
            {
                PlayFirework();
            });
    }
}
