using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class AbilityBase : MonoBehaviour
{
    public string AbilityName;
    public Entity Entity;

    public abstract void Init(Entity _entity);
    public abstract void DoAbility();
    public abstract void OnDeAbility();
}
