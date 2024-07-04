using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticObstacleTile : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.TryGetComponent(out CharacterBase characterBase);
            if (characterBase != null)
            {
                if (!characterBase.Invincible)
                {
                    Base.Manager.UI.FadeInOut(Base.Manager.Map.ReloadStage);
                }
                else
                {
                    gameObject.SetActive(false);
                }
            } 
        }
    }
}
