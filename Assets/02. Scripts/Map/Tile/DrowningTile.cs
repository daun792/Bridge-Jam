using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrowningTile : MonoBehaviour
{
    public bool IsDrwoning = false;
    private Vector3 startPos;
    private bool isDrowned = false;

    private void Start()
    {
        startPos = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && IsDrwoning)
        {
            transform.DOKill();
            transform.DOMoveY(startPos.y - 0.5f, 1f).SetEase(Ease.OutQuad);
            isDrowned = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isDrowned)
        {
            transform.DOKill();
            transform.DOMoveY(startPos.y, 1f).SetEase(Ease.OutBounce);
            isDrowned = false;
        }
    }
}
