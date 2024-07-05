using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage10 : StageBase
{
    [SerializeField] Transform cameraTrans;
    [SerializeField] Transform playerTrans;
    [SerializeField] GameObject colliderObj;
    [SerializeField] Transform blinkTrans;
    private Camera cam;

    WaitForSeconds wait;

    private Vector3 startPosition = new(466, 0, -10);
    private Vector3 cameraPosition = new(479.45f, -1.37f, -10);

    private float startSize = 8f;
    private float cameraSize = 0.67f;

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
        colliderObj.SetActive(true);
        // 플레이어 못움직이게
        StartCoroutine(PlayDrugProduction());
    }

    private IEnumerator PlayDrugProduction()
    {
        Base.Manager.PostProcessing.SetGlitch(0.1f);

        yield return wait;

        Base.Manager.PostProcessing.SetGlitch(0.3f);
        cameraTrans.position = cameraPosition;
        cam.orthographicSize = cameraSize;
        Base.Manager.Sound.PlayBGM("BGM_Stage10");

        yield return wait;

        Base.Manager.PostProcessing.SetGlitch(0.2f);
        cameraTrans.position = startPosition;
        cam.orthographicSize = startSize;

        yield return wait;

        Base.Manager.PostProcessing.SetGlitch(0.4f);
        cameraTrans.position = cameraPosition;
        cam.orthographicSize = cameraSize;

        yield return wait;

        playerTrans.position = blinkTrans.position;

        Base.Manager.PostProcessing.SetGlitch(0.1f);
        cameraTrans.position = startPosition;
        cam.orthographicSize = startSize;

        //플레이어 움직일 수 있게
    }
}
