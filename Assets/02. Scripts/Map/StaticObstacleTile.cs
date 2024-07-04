using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticObstacleTile : MonoBehaviour
{
    private SpriteRenderer sprite;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Base.Manager.Map.ReloadStage();
        }
    }
}
