using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Spine;
using Spine.Unity;
using static UnityEngine.GraphicsBuffer;

public abstract class Entity : MonoBehaviour
{
    [Header("Status /SeeOnly")]
    public bool IsTheirTurn = false;
    public AttributeData AttributeData;

    [Header("References")]
    public HitBox HeadCollider;
    public HitBox SideCollider;
    public Health HP;
    public SkeletonAnimation SkAnimation;

    public Transform Target;
    public GameObject ProjectTile;

    public Action<bool> OnTheirTurn;

    public virtual void Init()
    {
        HP = GetComponent<Health>();
        HP.InitHealth(AttributeData);

        SkAnimation = GetComponent<SkeletonAnimation>();
    }

    public void SetAnimationOnce(string _animation)
    {
        SkAnimation.AnimationState.SetAnimation(0, _animation, false).Complete += _trackEntry =>
        {
            SkAnimation.AnimationState.SetAnimation(0, "Idle UnFriendly 1", true);
        };
    }

    public void SetAnimationLoop(string _animation) => SkAnimation.AnimationState.SetAnimation(0, _animation, true);

    public abstract void SetAttribute(AttributeData attributeData);

    public virtual void Turn()
    {
       
    }

    public virtual void EndTurn()
    {
        IsTheirTurn = false;
    }

    public virtual void DamageSide()
    {

    }

    public virtual void DamageHead()
    {

    }

    public void Shoot(float _value, bool _isCalWind = false, bool _isPowerThrow = false, bool _isDouble = false)
    {
        Rigidbody2D _projectileInstance = Instantiate(ProjectTile, HeadCollider.transform.position, Quaternion.identity).GetComponent<Rigidbody2D>();
        _projectileInstance.GetComponent<ProjectileBase>().Entity = Target.GetComponent<Entity>();

        Vector2 _direction = (Vector2)Target.position - (Vector2)transform.position;

        float directionSign = Mathf.Sign(_direction.x);

        float _distance = _direction.magnitude;
        float _gravity = Physics2D.gravity.magnitude;
        float _verticalDistance = _direction.y;

        float _initialVelocityY = Mathf.Sqrt(2 * _gravity * 2f);

        float _time = (_initialVelocityY + Mathf.Sqrt(_initialVelocityY * _initialVelocityY + 2 * _gravity * _verticalDistance)) / _gravity;

        float _initialVelocityX = (_distance / _time) * directionSign;

        Vector2 _initialVelocity = new Vector2(_initialVelocityX, _initialVelocityY) * _value;

        if (_isCalWind)
        {
            Debug.Log("Wind used");
            float _windDirection = Mathf.Sign(GameManager.Instance.WindForce);
            Vector2 _windForce = new Vector2(_windDirection, 0) * Mathf.Abs(GameManager.Instance.WindForce) * 2;
            _initialVelocity += _windForce;
        }

        _projectileInstance.velocity = _initialVelocity;
    }
}
