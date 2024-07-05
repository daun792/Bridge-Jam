using UnityEngine;

public class CharacterPlayer : CharacterBase
{
    float moveAxis;
    bool jumpButtonDown;

    #region Unity Events
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void Update()
    {
        GetInput();
        base.Update();
    }
    #endregion

    protected override void BehaveOnGround()
    {
        base.BehaveOnGround();
        Move(moveAxis);
        if (jumpButtonDown)
        {
            Jump();
        }
    }

    protected override void BehaveInAir()
    {
        base.BehaveInAir();
        Move(moveAxis);
        if (jumpButtonDown && IsSpace)
        {
            Jump();
        }
    }

    private void GetInput()
    {
        moveAxis = (Input.GetKey(KeyCode.A) ? -1 : 0) + (Input.GetKey(KeyCode.D) ? 1 : 0);
        jumpButtonDown = Input.GetKeyDown(KeyCode.Space);
    }
}
