using UnityEngine;
using DG.Tweening;
using TMPro;

public class TitleManager : MonoBehaviour
{
    [SerializeField] RectTransform backgroundPanel;
    [SerializeField] TextMeshProUGUI startDescTxt;

    private bool isKeyDown = true;

    private void Start()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(backgroundPanel.DOAnchorPosY(-3000f, 0f))
            .Append(backgroundPanel.DOAnchorPosY(-1080f, 1f).SetEase(Ease.OutCubic))
            .OnComplete(() => isKeyDown = false);

        startDescTxt.DOFade(0f, 1.5f).SetEase(Ease.InCubic).SetLoops (-1, LoopType.Yoyo);
        startDescTxt.GetComponent<RectTransform>().DOAnchorPosY(150f, 1.5f).SetEase(Ease.InCubic).SetLoops (-1, LoopType.Yoyo);
    }

    private void Update()
    {
        if (Input.anyKeyDown && !isKeyDown)
        {
            isKeyDown = true;

            LoadGameScene();
        }
    }

    private void LoadGameScene()
    {
        startDescTxt.DOKill();

        Sequence sequence = DOTween.Sequence();
        sequence.Append(startDescTxt.DOFade(0f, 1f))
            .Append(backgroundPanel.DOAnchorPosY(1080f, 1f).SetEase(Ease.InCubic))
            .OnComplete(() => Base.LoadScene(SceneName.Game));
    }
}
