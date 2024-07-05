using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBase : MonoBehaviour
{
    private bool isPlayerEnter = false;
    public static Action InitObstacle;

    protected virtual void Start()
    {
        InitObstacle += Init;
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isPlayerEnter)
        {
            collision.gameObject.TryGetComponent(out CharacterBase characterBase);
            if (characterBase != null)
            {
                isPlayerEnter = true;

                if (!characterBase.Invincible)
                {
                    StartCoroutine(Base.Manager.Map.PlayerDead());
                }
                else
                {
                    gameObject.SetActive(false);
                }
            } 
        }
        else
        {
            CheckOtherCollision(collision);
        }
    }

    private void Init()
    {
        isPlayerEnter = false;
    }

    protected virtual void CheckOtherCollision(Collision2D collision) { }
}
