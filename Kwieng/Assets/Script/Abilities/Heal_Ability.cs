using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal_Ability : AbilityBase
{
    public override void Init(Entity _entity)
    {
        Entity = _entity;
    }
    public override void DoAbility()
    {
        Health _hp = Entity.GetComponent<Health>();
        _hp.Heal(CSVDataLoader.Instance.AttributeDataDict["Heal"].HP);
    }

    public override void OnDeAbility()
    {
        
    }
}
