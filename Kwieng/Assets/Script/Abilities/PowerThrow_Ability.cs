using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerThrow_Ability : AbilityBase
{
    public override void Init(Entity _entity)
    {
        Entity = _entity;
    }

    public override void DoAbility()
    {
        Entity.PowerThrow = true;
    }

    public override void OnDeAbility()
    {
        
    }
}
