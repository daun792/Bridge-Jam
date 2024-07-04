using System;
using UnityEngine;

public class StageBase : MonoBehaviour
{
    [Serializable]
    struct StageComposition
    {
        public GameObject stageParent;
        public Transform startPosition;
        public Transform disappearTileParent;
    }

    [SerializeField] StageComposition cleanStage;
    [SerializeField] StageComposition drugStage;

    private bool isClean = true;
    private StageComposition currStage;

    public int StageIndex;

    private void Awake()
    {
        cleanStage.stageParent.SetActive(false);
        drugStage.stageParent.SetActive(false);
    }

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
        if (currStage.disappearTileParent != null)
        {
            var disappearTiles = currStage.disappearTileParent.GetComponentsInChildren<SpriteRenderer>();
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

    
   

    // Update is called once per frame
    void Update()
    {
        
    }
}
