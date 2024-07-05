using System;
using UnityEngine;

public abstract class StageBase : MonoBehaviour
{
    [Serializable]
    protected struct StageComposition
    {
        public GameObject stageParent;
        public Transform startPosition;
        public Transform disappearTileParent;
    }

    [SerializeField] protected StageComposition cleanStage;
    [SerializeField] protected StageComposition drugStage;
    [SerializeField] int stageIndex;
    [SerializeField] float stageTime;

    protected bool isClean = true;
    private StageComposition currStage;

    public int StageIndex { get; private set; }
    public float StageTime { get; private set; }

    protected virtual void Awake()
    {
        cleanStage.stageParent.SetActive(false);
        drugStage.stageParent.SetActive(false);

        StageIndex = stageIndex;
        StageTime = stageTime;
    }

    public virtual void SetStage(bool _isClean)
    {
        isClean = _isClean;
        currStage = _isClean ? cleanStage : drugStage;

        currStage.stageParent.SetActive(true);
    }

    public virtual Vector3 GetStartPosition() => isClean switch
    {
        true => cleanStage.startPosition.position,
        false => drugStage.startPosition.position
    };

    public virtual float GetCameraPositionY() => isClean switch
    {
        true => 0f,
        false => -20f
    };

    public virtual float GetMiddleCameraPositionX() => 0f;

    public virtual void ResetStage()
    {
        if (currStage.disappearTileParent != null)
        {
            var disappearTiles = currStage.disappearTileParent.GetComponentsInChildren<SpriteRenderer>(true);
            var fullAlpha = new Color(0f, 0f, 0f, 1f);

            foreach (var tile in disappearTiles)
            {
                tile.gameObject.SetActive(true);
                tile.color += fullAlpha;
            }
        }

        ObstacleBase.InitObstacle?.Invoke();
    }

    public abstract void UseDrug();
}
