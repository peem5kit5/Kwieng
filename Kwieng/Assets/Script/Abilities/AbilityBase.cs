using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class AbilityBase : MonoBehaviour
{
    public Action OnAbility;
    public Action OnFinishAbility;

    public AttributeData AttributeData;

    public virtual void Init()
    {
        OnAbility += OnDoAbility;
        OnFinishAbility += OnDeAbility;
    }

    public abstract void OnDoAbility();
    public abstract void OnDeAbility();
}
