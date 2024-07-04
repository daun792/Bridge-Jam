using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(EndGame());
        }
    }

    public IEnumerator EndGame()
    {
        Base.Manager.UI.FadeIn();

        yield return new WaitForSeconds(1f);

        var sceneName = Base.Manager.Map.IsClean ? SceneName.TrueEnd : SceneName.BadEnd;
        Base.LoadScene(sceneName);
    }
}
