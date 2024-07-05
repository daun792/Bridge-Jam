using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBase : MonoBehaviour
{
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.TryGetComponent(out CharacterBase characterBase);
            if (characterBase != null)
            {
                if (!characterBase.Invincible)
                {
                    Base.Manager.Map.PlayerDead();
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

    protected virtual void CheckOtherCollision(Collision2D collision) { }
}
