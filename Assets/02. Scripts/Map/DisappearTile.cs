using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DisappearTile : MonoBehaviour
{
    private SpriteRenderer sprite;
    private BoxCollider2D collision;

    private readonly float delay = 1f;
    private bool isPlayerOnTile = false;

    private void OnCollisionEnter2D(Collision2D _collision)
    {
        if (_collision.gameObject.CompareTag("Player") && !isPlayerOnTile)
        {
            isPlayerOnTile = true;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(sprite.DOFade(0f, delay).SetEase(Ease.Linear))
                .OnComplete(() => gameObject.SetActive(false));
        }
    }

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        collision = GetComponent<BoxCollider2D>();
    }
}
