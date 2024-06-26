using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleAttack_Ability : AbilityBase
{
    public override void Init(Entity _entity)
    {
        Entity = _entity;
    }

    public override void DoAbility()
    {
        Entity.Double = true;
    }

    public override void OnDeAbility()
    {
        
    }
}
