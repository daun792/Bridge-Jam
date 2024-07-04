using System;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public enum CharacterState
    {
        OnGround,
        InAir,
    }
    
    #region Events
    public event Action<float> OnMoveEvent;
    #endregion

    public float MovementSpeed = 1;
    public float JumpVelocity = 1;
    public float GravityScale = 1;
    public LayerMask groundMask;
    public bool Invincible = false;

    public bool IsFlip
    {
        get => characterSpriteRenderer.flipX;
        set => characterSpriteRenderer.flipX = value;
    }

    protected CharacterState characterState = CharacterState.OnGround;
    protected Rigidbody2D characterRigidbody;
    protected Collider2D characterCollider;
    protected SpriteRenderer characterSpriteRenderer;
    protected Animator characterAnimator;
    protected CharacterBase focusedCharacter;

    private Transform currentFloor;
    protected Transform CurrentFloor
    {
        get => currentFloor;
        set
        {
            transform.SetParent(value);
            currentFloor = value;
        }
    }

    private float speedCoefficient = 4f;
    private float jumpVelocityCoefficient = 14.4f;
    private float gravityCoefficient = 30f;
    private Vector2 velocity = Vector2.zero;
    private Vector2 gravity = Vector2.zero;
    private Vector2 groundNormal;
    private float angle;
    private bool onGround = false;
    private bool onSlope = false;
    private Vector2 pausedVelocity;

    private string animatorCharacterSpeedName = "CharacterSpeed";
    private string animatorOnGroundName = "OnGround";
    private string animatorInAirName = "InAir";

    #region Unity Events
    protected virtual void OnEnable()
    {
        characterRigidbody = GetComponentInChildren<Rigidbody2D>();
        characterCollider = GetComponentInChildren<Collider2D>();
        characterSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        characterAnimator = GetComponentInChildren<Animator>();
    }

    protected virtual void Update()
    {
        BehaveByState();
    }
    #endregion

    #region Movement
    public virtual void Move(float value)
    {
        value = Mathf.Clamp(value, -1, 1);
        velocity.x = MovementSpeed * speedCoefficient * value;
        if (onSlope)
        {
            velocity.y = (groundNormal.x * value > 0 ? -1 : 1) * Mathf.Abs(velocity.x) * Mathf.Tan(angle * Mathf.Deg2Rad);
        }
        else
        {
            velocity.y = 0;
        }
        
        characterAnimator.SetFloat(animatorCharacterSpeedName, value > 0 ? value : -value);
        if (characterState == CharacterState.OnGround)
        {
            characterAnimator.speed = value > 0 ? value : -value;
        }
        else
        {
            characterAnimator.speed = 1;
        }
        
        if (value == 0)
        {
            characterRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            characterRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            if (characterSpriteRenderer.flipX && value > 0)
            {
                characterSpriteRenderer.flipX = false;
            }
            else if (!characterSpriteRenderer.flipX && value < 0)
            {
                characterSpriteRenderer.flipX = true;
            }
        }
        OnMoveEvent?.Invoke(value);
    }

    public virtual void Jump()
    {
        Base.Manager.Sound.PlaySFX("SFX_PlayerJump");
        gravity.y = JumpVelocity * GravityScale * jumpVelocityCoefficient;
    }

    protected virtual void EffectedByGravity()
    {
        gravity.y -= GravityScale * gravityCoefficient * Time.deltaTime;
    }
    #endregion

    public virtual Vector2 GetVelocity()
    {
        return new Vector2(MovementSpeed * speedCoefficient, JumpVelocity * GravityScale * jumpVelocityCoefficient);
    }

    public virtual float GetGravityScale()
    {
        return GravityScale * gravityCoefficient;
    }

    public virtual Vector2 GetColliderSize()
    {
        return new Vector2(characterCollider.bounds.size.x * transform.localScale.x, characterCollider.bounds.size.y * transform.localScale.y);
    }

    protected virtual void BehaveByState()
    {
        CheckFloorExist();
        switch (characterState)
        {
            case CharacterState.OnGround:
                BehaveOnGround();
                break;
            case CharacterState.InAir:
                BehaveInAir();
                break;
        }
        characterRigidbody.velocity = velocity + gravity;
    }

    protected virtual void BehaveOnGround()
    {
        if (!onGround)
        {
            characterAnimator.SetTrigger(animatorInAirName);
            characterState = CharacterState.InAir;
        }
    }

    protected virtual void BehaveInAir()
    {
        EffectedByGravity();
        if (onGround)
        {
            characterAnimator.SetTrigger(animatorOnGroundName);
            characterState = CharacterState.OnGround;
            gravity.y = 0;
        }
    }

    public virtual void SetAnimator(RuntimeAnimatorController controller)
    {
        characterAnimator.runtimeAnimatorController = controller;
    }

    protected void MakeImmobile(bool rememberVelocity = true)
    {
        characterRigidbody.isKinematic = true;
        characterRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        pausedVelocity = rememberVelocity ? pausedVelocity : Vector2.zero;
        characterAnimator.enabled = false;
        StopOnPosition();
    }

    protected void MakeMobile()
    {
        characterRigidbody.isKinematic = false;
        characterRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        characterRigidbody.velocity = pausedVelocity;
        characterAnimator.enabled = true;
    }

    private void StopOnPosition()
    {
        velocity = Vector2.zero;
        characterAnimator.SetFloat(animatorCharacterSpeedName, 0);
    }

    private void CheckFloorExist()
    {
        Vector3[] groundCheckCenters = {
            characterCollider.bounds.center,
            characterCollider.bounds.center + new Vector3(characterCollider.bounds.size.x * 0.15f, 0, 0),
            characterCollider.bounds.center + new Vector3(-characterCollider.bounds.size.x * 0.15f, 0, 0),
        };
        RaycastHit2D hit;
        foreach (Vector3 center in groundCheckCenters)
        {
            hit = Physics2D.Raycast(
                center,
                Vector2.down,
                characterCollider.bounds.size.y * 0.55f,
                groundMask);

            if (hit.collider != null)
            {
                CurrentFloor = hit.collider.transform;
                groundNormal = hit.normal;
                angle = Vector2.Angle(groundNormal, Vector2.up);
                onSlope = angle != 0;
                onGround = true;
                return;
            }
        }
        CurrentFloor = null;
        onSlope = false;
        onGround = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        RaycastHit2D hit = Physics2D.Raycast(
            characterCollider.bounds.center + Vector3.up * characterCollider.bounds.size.y * 0.4f,
            Vector2.up,
            characterCollider.bounds.size.y * 0.55f,
            ~0 - 1);
        Debug.DrawRay(characterCollider.bounds.center + Vector3.up * characterCollider.bounds.size.y * 0.55f, Vector2.up * characterCollider.bounds.size.y * 0.55f, Color.red, 1f);
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject);
            gravity.y = 0;
        }
    }
}
