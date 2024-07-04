using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrueEndingManager : MonoBehaviour
{
    [SerializeField] float moveTime = 0.5f;
    [SerializeField] Image frontPanel;
    [SerializeField] Transform player;
    [SerializeField] Transform[] waypoints;
    private int currentWaypointIndex = 0;

    private void Start()
    {
        frontPanel.DOFade(0f, 1f);
        //MoveToNextPoint();
        Invoke("LoadGameScene", 5f);
        StartCoroutine(Move8bit());
    }

    private void MoveToNextPoint()
    {
        player.DOMove(waypoints[currentWaypointIndex].position, moveTime).SetEase(Ease.Linear).
            OnComplete(() => MoveToNextPoint());
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    }

    private void LoadGameScene()
    {
        frontPanel.DOKill();

        Sequence sequence = DOTween.Sequence();
        sequence.Append(frontPanel.DOFade(1f, 1f))
            .OnComplete(() => Base.LoadScene(SceneName.Title));
    }

    private IEnumerator Move8bit()
    {
        while (true)
        {
            while (player.transform.position.y < waypoints[1].position.y)
            {
                player.transform.position += Vector3.up * 0.08f;
                yield return new WaitForSeconds(0.02f);
            }
            while (player.transform.position.y > waypoints[0].position.y)
            {
                player.transform.position -= Vector3.up * 0.08f;
                yield return new WaitForSeconds(0.02f);
            }
        }
    }
}
