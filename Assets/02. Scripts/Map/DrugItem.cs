using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DrugItem : MonoBehaviour
{
    public UnityEvent OnUseDrug;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnUseDrug?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
