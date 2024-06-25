using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public enum PlayerState
    {
        Player1,
        Player2
    }

    public PlayerState PlayerNumber;

    public override void Turn()
    {
        IsTheirTurn = (int)PlayerNumber == (int)GameManager.Instance.CurrentTurn;
        base.Turn();
    }

    public override void Init()
    {
        base.Init();
        if (PlayerNumber == PlayerState.Player1)
        {
            HeadCollider = GameObject.Find("HeadPoint_Aunt").GetComponent<HitBox>();
            SideCollider = GameObject.Find("SidePoint_Aunt").GetComponent<HitBox>();

            Target = GameManager.Instance.Soldier.transform;
            ProjectTile = GameManager.Instance.AuntProjectile;
        }
        else
        {
            HeadCollider = GameObject.Find("HeadPoint_SoldierPig").GetComponent<HitBox>();
            SideCollider = GameObject.Find("SidePoint_SoldierPig").GetComponent<HitBox>();

            Target = GameManager.Instance.Aunt.transform;
            ProjectTile = GameManager.Instance.SoldierProjectile;
        }

        HeadCollider.SetHitBox(this);
        SideCollider.SetHitBox(this);
    }

    public void SetPlayer(PlayerState _playerNumber) => PlayerNumber = _playerNumber;

    public override void SetAttribute(AttributeData _attributeData) => AttributeData = _attributeData;
}
