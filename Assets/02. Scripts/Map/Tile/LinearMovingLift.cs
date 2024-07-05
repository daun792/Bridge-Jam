using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearMovingLift : MonoBehaviour
{
    [SerializeField] float moveTime = 3f;
    [SerializeField] Transform[] waypoints;
    private int currentWaypointIndex = 0;

    private void Start()
    {
        MoveToNextPoint();
    }

    private void MoveToNextPoint()
    {
        transform.DOMove(waypoints[currentWaypointIndex].position, moveTime).SetEase(Ease.Linear).
            OnComplete(() => MoveToNextPoint());
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    protected void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(Base.Manager.Map.transform);
        }
    }
}
