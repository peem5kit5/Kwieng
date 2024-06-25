using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private UI_Controller uiController;

    public void OnHowToClick()
    {
        uiController.Activate_HowToPlay(true);
    }

    public void OnStartGameClick()
    {
        uiController.DeactivateAll();
        uiController.Activate_PlayerOrBot(true);
    }

    public void OnPlayerClick()
    {
        uiController.DeactivateAll();
        GameManager.Instance.InitGameState_VsPlayer();
    }

    public void OnBotClick()
    {
        uiController.DeactivateAll();
        uiController.Activate_Difficulty(true);
        GameManager.Instance.InitGameState_VsBot();
    }
}
