using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileBase : MonoBehaviour
{
    public virtual void OnTriggerEnter2D(Collider2D _collision)
    {
        
    }
}
