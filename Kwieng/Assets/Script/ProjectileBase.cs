using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileBase : MonoBehaviour
{
    public string TargetSortingLayer;
    public Entity Entity;
    public float ExpiredTime = 4;

    public virtual void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.CompareTag(TargetSortingLayer))
        {
            var _hitBox = _collision.GetComponent<HitBox>();

            if (_hitBox != null)
            {
                _hitBox.Damaging();
                GameManager.Instance.EndTurn();

                Destroy(gameObject);
            }
        }

        if (_collision.CompareTag("Wall"))
        {
            Entity.SetAnimationOnce("Happy Friendly");
            GameManager.Instance.EndTurn();

            Destroy(gameObject);
        }
    }

    private void Update()
    {
        ExpiredTime -= Time.deltaTime;

        if (ExpiredTime <= 0)
            SelfDestroy();
    }

    private void SelfDestroy()
    {
        Entity.SetAnimationOnce("Happy Friendly");
        GameManager.Instance.EndTurn();

        Destroy(gameObject);
    }
}
