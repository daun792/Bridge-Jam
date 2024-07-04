using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DrugItem : MonoBehaviour
{
    [SerializeField] int itemIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Base.Manager.Map.UseDrug();
            Base.Manager.UI.ActiveItem(itemIndex);
            gameObject.SetActive(false);
        }
    }
}
