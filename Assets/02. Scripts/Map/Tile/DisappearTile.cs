using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DisappearTile : MonoBehaviour
{
    private SpriteRenderer sprite;

    private readonly float delay = 1f;
    private bool isPlayerOnTile = false;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        isPlayerOnTile = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isPlayerOnTile)
        {
            float half = collision.gameObject.GetComponent<Collider2D>().bounds.size.y * 0.4f;
            if (collision.transform.position.y - half > transform.position.y)
            {
                isPlayerOnTile = true;

                Base.Manager.Sound.PlaySFX("SFX_Tile_Disappear");

                Sequence sequence = DOTween.Sequence();
                sequence.Append(sprite.DOFade(0f, delay).SetEase(Ease.Linear))
                    .OnComplete(TileDisappear);
            }
        }
    }

    public void TileDisappear()
    {
        foreach (Transform child in transform)
        {
            child.SetParent(null);
        }
        gameObject.SetActive(false);
    }
}
