using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageBase : MonoBehaviour
{
    struct StageComposition
    {
        public readonly GameObject stageParent;
        public readonly Transform startPosition;
        public readonly SpriteRenderer[] disappearTiles;
    }

    [SerializeField] StageComposition cleanStage;
    [SerializeField] StageComposition drugStage;

    private bool isClean = true;
    private StageComposition currStage;

    public int StageIndex { get; protected set; }

    public void SetStage(bool _isClean)
    {
        isClean = _isClean;
        currStage = _isClean ? cleanStage : drugStage;

        currStage.stageParent.SetActive(true);
    }

    public Vector3 GetStartPosition() => isClean switch
    {
        true => cleanStage.startPosition.position,
        false => drugStage.startPosition.position
    };

    public void ResetStage()
    {
        if (currStage.disappearTiles.Length > 0)
        {
            var disappearTiles = currStage.disappearTiles;
            var fullAlpha = new Color(0f, 0f, 0f, 1f);

            foreach (var tile in disappearTiles)
            {
                tile.gameObject.SetActive(true);
                tile.color += fullAlpha;
            }
        }
    }

    public virtual void UseDrug()
    {

    }

    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
