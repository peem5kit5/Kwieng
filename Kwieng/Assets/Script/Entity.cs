using System.Collections;
using UnityEngine;
using System;
using Spine.Unity;

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

    [Header("Buff")]
    public bool PowerThrow;
    public bool Double;

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

    public void SetAnimationLoop(string _animation) 
    {
        SkAnimation.AnimationState.ClearTracks();
        SkAnimation.AnimationState.SetAnimation(0, _animation, true);
    } 

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

    public void Shoot(float _value, bool _isCalWind = false)
    {
        Rigidbody2D _projectileInstance = Instantiate(ProjectTile, HeadCollider.transform.position, Quaternion.identity).GetComponent<Rigidbody2D>();
        var _projectTileBase = _projectileInstance.GetComponent<ProjectileBase>();
        _projectTileBase.Entity = Target.GetComponent<Entity>();

        if (PowerThrow)
        {
            _projectTileBase.SetDamageAddition(CSVDataLoader.Instance.AttributeDataDict["PowerThrow"].Damage);
            PowerThrow = false;
        }

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
            float _windDirection = Mathf.Sign(GameManagers.Instance.WindForce);
            Vector2 _windForce = new Vector2(_windDirection, 0) * Mathf.Abs(GameManagers.Instance.WindForce) * 2;
            _initialVelocity += _windForce;
        }

        _projectileInstance.velocity = _initialVelocity;

        if (Double)
        {
            Double = false;
            _projectTileBase.ByPass = true;
            StartCoroutine(DoubleAttackDelay(_initialVelocity));
        }
    }

    private IEnumerator DoubleAttackDelay( Vector2 _dir)
    {
        yield return new WaitForSeconds(1);

        Rigidbody2D _doubleRb = Instantiate(ProjectTile, HeadCollider.transform.position, Quaternion.identity).GetComponent<Rigidbody2D>();

        if (PowerThrow)
        {
            _doubleRb.GetComponent<ProjectileBase>().SetDamageAddition(CSVDataLoader.Instance.AttributeDataDict["PowerThrow"].Damage);
            PowerThrow = false;
        }

        _doubleRb.velocity = _dir;
    }
}
