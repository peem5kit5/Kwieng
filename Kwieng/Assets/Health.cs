using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Health : MonoBehaviour
{
    [Header("Status")]
    public int HP;
    public int MaxHP;

    [Header("UI")]
    [SerializeField] private Slider hpSlider;

    public Action<int> OnHPChange;
    
    public void InitHealth(AttributeData _attributeData)
    {
        MaxHP = _attributeData.HP;
        HP = MaxHP;

        OnHPChange += Damage;

        hpSlider.maxValue = MaxHP;
        hpSlider.SetValueWithoutNotify(MaxHP);
    }

    public void Damage(int _damage)
    {
        HP -= _damage;
        hpSlider.SetValueWithoutNotify(HP);

        if (HP <= 0)
            GameManagers.Instance.ConclusionGame(gameObject.name);
    }

    public void Heal(int _heal)
    {
        if (HP >= MaxHP)
            HP = MaxHP;
        else
            HP += _heal;

        hpSlider.SetValueWithoutNotify(HP);
    }
}
