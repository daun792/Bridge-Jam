using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BadEndingManager : MonoBehaviour
{
    [SerializeField] Image frontPanel;
    [SerializeField] TMP_Text text;
    [SerializeField] Transform player;
    [SerializeField] Transform last;

    private void Start()
    {
        frontPanel.DOFade(0f, 1f);
        Invoke("LoadGameScene", 5f);
        StartCoroutine(Move8bit());
        StartCoroutine(ChangeTitle());
    }

    private void LoadGameScene()
    {
        frontPanel.DOKill();

        Sequence sequence = DOTween.Sequence();
        sequence.Append(frontPanel.DOFade(1f, 1f))
            .OnComplete(() => Base.LoadScene(SceneName.Title));
    }

    private IEnumerator Move8bit()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                player.transform.position += Vector3.right * 0.08f;
                yield return new WaitForSeconds(0.02f);
            }
            yield return new WaitForSeconds(1f);
        }
        player.DOScaleY(0.5f, 1f);
        player.DOMove(last.position, 1f);
    }

    private IEnumerator ChangeTitle()
    {
        yield return new WaitForSeconds(1f);
        text.SetText("BAD TRIP");
        yield return new WaitForSeconds(0.05f);
        text.SetText("HAPPY TRIP");
        yield return new WaitForSeconds(1f);
        text.SetText("BAD TRIP");
        yield return new WaitForSeconds(0.05f);
        text.SetText("HAPP? TRIP");
        yield return new WaitForSeconds(1f);
        text.SetText("BAD TRIP");
        yield return new WaitForSeconds(0.05f);
        text.SetText("HAPPY TRIP");
    }
}
