using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage10 : StageBase
{
    [SerializeField] Transform cameraTrans;
    [SerializeField] Transform playerTrans;
    [SerializeField] GameObject colliderObj;
    [SerializeField] Transform blinkTrans;
    [SerializeField] GameObject obstaclesParent;
    private Camera cam;

    WaitForSeconds wait;

    private Vector3 startPosition = new(466, 0, -10);
    private Vector3 cameraPosition = new(479.51f, -1.44f, -10);
    private readonly Vector3 endPosition = new(472.8f, -1, -10);

    private float startSize = 8f;
    private float cameraSize = 0.6f;

    protected override void Awake()
    {
        base.Awake();

        wait = new(1f);
        cam = cameraTrans.GetComponent<Camera>();
    }

    public override float GetCameraPositionY() => 0f;

    public override float GetMiddleCameraPositionX() => 466f;

    public override void UseDrug()
    {
        obstaclesParent.SetActive(false);
        colliderObj.SetActive(true);
        Base.Manager.Map.StopWindowLotation();
        Base.Manager.Map.StopTimeBacking();
        Base.Manager.Map.StopTimer();

        cam.ResetProjectionMatrix();

        StartCoroutine(PlayDrugProduction());
    }

    private IEnumerator PlayDrugProduction()
    {
        Base.Manager.PostProcessing.SetGlitch(0.1f);

        yield return wait;

        cam.orthographicSize = cameraSize;
        Base.Manager.PostProcessing.SetGlitch(0.3f);
        cameraTrans.position = cameraPosition;
        Base.Manager.Sound.PlayBGM("BGM_Stage10");

        yield return wait;

        cam.orthographicSize = startSize;
        Base.Manager.PostProcessing.SetGlitch(0.2f);
        cameraTrans.position = startPosition;

        yield return wait;

        cam.orthographicSize = cameraSize;
        Base.Manager.PostProcessing.SetGlitch(0.4f);
        cameraTrans.position = cameraPosition;

        yield return wait;

        playerTrans.position = blinkTrans.position;

        cam.orthographicSize = startSize/2;
        Base.Manager.PostProcessing.SetGlitch(0.1f);
        cameraTrans.position = endPosition;
    }
}
