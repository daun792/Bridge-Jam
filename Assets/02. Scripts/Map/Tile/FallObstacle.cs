using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FallObstacle : ObstacleBase
{
    [SerializeField] float startDelay = 0f;
    [SerializeField] float fallingDuration = 2f;

    private Vector3 startPosition;
    private Sequence sequence;
    private float delay = 3f;

    protected override void Start()
    {
        base.Start();

        startPosition = transform.position;
    }

    private void OnEnable()
    {
        StartCoroutine(StartDelay());
    }

    private void OnDisable()
    {
        sequence?.Kill();
    }

    private IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(startDelay);
        StartFall();
    }

    private void StartFall()
    {
        sequence = DOTween.Sequence();

        sequence.SetAutoKill(false)
            .AppendInterval(delay)
           .AppendCallback(() =>
           {
               Base.Manager.Sound.PlaySFX("SFX_Tile_FallObstacle");
               gameObject.SetActive(true);
               transform.position = startPosition;
           })
           .Append(transform.DOLocalMoveY(-20, fallingDuration)).SetEase(Ease.Linear);
    }

    protected override void CheckOtherCollision(Collision2D collision)
    {
        sequence?.Kill();
        gameObject.SetActive(false);
        StartFall();
    }

    public void SetItemEffect()
    {
        delay = 6f;
    }
}
