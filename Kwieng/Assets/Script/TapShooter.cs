using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TapShooter : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public bool IsPlaying = false;
    public bool IsBot = false;
    public bool Shooted = false;
    public float PowerCharge;
    public float MaxPowerCharge = 1.5f;
    public bool ReverseCharge;

    [SerializeField] private Entity CurrentEntityToShoot;
    [SerializeField] private Slider currentSlider;

    [SerializeField] private Slider auntChargeSlider;
    [SerializeField] private Slider soldierChargeSlider;

    private bool isCharging = false;

    public void ValidateEntity(Entity _entity)
    {
        CurrentEntityToShoot = _entity;

        if (CurrentEntityToShoot is BotBehaviour _bot)
        {
            currentSlider = soldierChargeSlider;
        }

        if (CurrentEntityToShoot is Player _player)
        {
            if (_player.PlayerNumber == Player.PlayerState.Player1)
                currentSlider = auntChargeSlider;
            else
                currentSlider = soldierChargeSlider;
        }
        currentSlider.maxValue = MaxPowerCharge;
        PowerCharge = 0;
    }

    void Update()
    {
        if (CurrentEntityToShoot is BotBehaviour)
            IsBot = true;
        else
            IsBot = false;

        if (!GameManagers.Instance.IsGameFinished)
        {
            if (isCharging && IsPlaying && !IsBot && !Shooted)
            {
                if (!ReverseCharge)
                {
                    PowerCharge += Time.deltaTime * 5;
                    if (PowerCharge >= MaxPowerCharge)
                    {
                        PowerCharge = MaxPowerCharge;
                        ReverseCharge = true;
                    }
                }
                else
                {
                    PowerCharge -= Time.deltaTime * 5;
                    if (PowerCharge <= 0)
                    {
                        PowerCharge = 0;
                        ReverseCharge = false;
                    }
                }

                currentSlider.SetValueWithoutNotify(PowerCharge);
            }
        }
    }

    public void OnPointerDown(PointerEventData _pointerEvent)
    {
        if (IsPlaying && !IsBot && !Shooted)
            isCharging = true;
    }

    public void OnPointerUp(PointerEventData _pointerEvent)
    {
        if (IsPlaying && !IsBot && !Shooted)
        {
            isCharging = false;
            CurrentEntityToShoot.Shoot(PowerCharge, true);
            PowerCharge = 0;
            Shooted = true;
        }
    }

    public void OnPointerExit(PointerEventData _pointerEvent)
    {
        if (IsPlaying && !IsBot)
        {
            isCharging = false;
            CurrentEntityToShoot.Shoot(PowerCharge, true);
            PowerCharge = 0;
            Shooted = true;
        }
    }
}
