using UnityEngine;

public class BotBehaviour : Entity
{
    public bool Shooted;
    public override void Init()
    {
        OnTheirTurn += OnTurn;

        Target = GameManagers.Instance.Aunt.transform;
        ProjectTile = GameManagers.Instance.SoldierProjectile;

        HeadCollider = GameObject.Find("HeadPoint_SoldierPig").GetComponent<HitBox>();
        SideCollider = GameObject.Find("SidePoint_SoldierPig").GetComponent<HitBox>();

        HeadCollider.SetHitBox(this);
        SideCollider.SetHitBox(this);

        base.Init();
    }

    public override void Turn()
    {
        IsTheirTurn = (int)GameManagers.Instance.CurrentTurn == 1;
        GameManagers.Instance.TapShooter.IsBot = IsTheirTurn;
        Shooted = false;

        OnTurn(IsTheirTurn);
        base.Turn();
    }

    public override void EndTurn()
    {
        base.EndTurn();
    }

    private void OnTurn(bool _isTurn)
    {
        if (!_isTurn) return;

        if (GameManagers.Instance.IsGameFinished) return;

        if(!Shooted)
            CalculateAndShoot();
    }

    private void CalculateAndShoot()
    {
        int _random = Random.Range(0, 100);
        Debug.Log(_random);
        if(_random >= AttributeData.MissedChance)
        {
            Shoot(2.3f, false);
        }
        else
        {
            float _randomShootValue = Random.Range(3, 5);
            Shoot(_randomShootValue, true);
        }

        Shooted = true;
    }

    public override void SetAttribute(AttributeData _attributeData) => AttributeData = _attributeData;
}
