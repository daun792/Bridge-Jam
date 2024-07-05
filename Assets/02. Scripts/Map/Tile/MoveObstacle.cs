using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveObstacle : ObstacleBase
{
    [SerializeField] float startDelay = 0f;
    [SerializeField] float appearTime = 0.5f;
    [SerializeField] float appearDelay = 0.3f;
    [SerializeField] float disappearTime = 0.2f;
    [SerializeField] float disappearDelay = 1.5f;

    private void OnEnable()
    {
        StartCoroutine(StartMove());
    }

    private IEnumerator StartMove()
    {
        var appear = appearTime + appearDelay;
        var disappear = disappearTime + disappearDelay;

        var appearWait = new WaitForSeconds(appear);
        var disappearWait = new WaitForSeconds(disappear);

        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            transform.DOLocalMoveY(-1, appearTime).SetEase(Ease.Linear);
            yield return appearWait;

            transform.DOLocalMoveY(0, disappearTime).SetEase(Ease.Linear);
            yield return disappearWait;
        }
    }
}
