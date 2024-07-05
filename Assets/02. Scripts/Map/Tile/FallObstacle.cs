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

    private void Start()
    {
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
        Debug.Log("DDDDD");
        yield return new WaitForSeconds(startDelay);
        StartFall();
    }

    private void StartFall()
    {
        sequence = DOTween.Sequence();

        sequence.SetAutoKill(false)
           .AppendCallback(() =>
           {
               transform.position = startPosition;
           })
           .Append(transform.DOLocalMoveY(-20, fallingDuration)).SetEase(Ease.Linear);
    }

    protected override void CheckOtherCollision(Collision2D collision)
    {
        sequence?.Kill();
        StartFall();
    }
}
