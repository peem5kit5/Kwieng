using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public enum Hit
    {
        Side,
        Head
    }
    public Hit Type;

    [SerializeField] private Entity entity;
    [SerializeField] private Health hp;
    [SerializeField] private int damage;

    public void SetHitBox(Entity _entity) 
    {
        entity = _entity;
        hp = entity.GetComponent<Health>();

        var _csv = CSVDataLoader.Instance;

        if (Type == Hit.Head)
            damage = _csv.AttributeDataDict["NormalAttack"].Damage;
        else
            damage = _csv.AttributeDataDict["SmallAttack"].Damage;
    }

    public void Damaging(int _bonusDamage = 0) 
    {
        hp.OnHPChange.Invoke(damage += _bonusDamage);

        if (hp.HP <= 0)
            return;

        if (Type == Hit.Head)
            entity.SetAnimationOnce("Sleep UnFriendly");
        else
            entity.SetAnimationOnce("Moody UnFriendly");
    } 
}
