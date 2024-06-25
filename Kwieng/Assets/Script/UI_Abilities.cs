using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Abilities : MonoBehaviour, IPointerClickHandler
{
    public AbilityBase AbilityBase;
    public Entity Entity;

    public void SetEntity(Entity _entity)
    {
        Entity = _entity;
        AbilityBase.Init(_entity);
    }

    public void OnPointerClick(PointerEventData _pointerEvent)
    {
        //Use
        AbilityBase.DoAbility();
        gameObject.SetActive(false);
    }
}
