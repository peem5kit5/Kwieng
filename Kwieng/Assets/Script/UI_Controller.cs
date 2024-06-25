using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Controller : MonoBehaviour
{
    [Header("References")]
    public Canvas Canvas;

    [Header("Panels")]
    [SerializeField] private GameObject difficultyPanel;
    [SerializeField] private GameObject gamePlayPanel;
    [SerializeField] private GameObject howToPlayPanel;
    [SerializeField] private GameObject selectMenuPanel;
    [SerializeField] private GameObject playerOrBotPanel;

    [Header("Abilities UI")]
    [SerializeField] private UI_Abilities[] AuntAbilities;
    [SerializeField] private UI_Abilities[] SoldierAbilities;

    private void Start()
    {
        Canvas.sortingLayerName = "Blocking";

        selectMenuPanel.SetActive(true);
        gamePlayPanel.SetActive(false);
        difficultyPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
    }

    public void Init()
    {
        foreach (var _abilities in AuntAbilities)
            _abilities.SetEntity(GameObject.Find("Aunt").GetComponent<Entity>());

        foreach (var _abilities in SoldierAbilities)
            _abilities.SetEntity(GameObject.Find("Soldier").GetComponent<Entity>());
    }

    private void OnValidate()
    {
        if (!difficultyPanel)
            difficultyPanel = GameObject.Find("DifficultyPanel");

        if (!gamePlayPanel)
            gamePlayPanel = GameObject.Find("GamePlayPanel");

        if (!howToPlayPanel)
            howToPlayPanel = GameObject.Find("HowToPlayPanel");
    }

    public void Activate_Menu(bool _value) => selectMenuPanel.SetActive(_value);
    public void Activate_Difficulty(bool _value) => difficultyPanel.SetActive(_value);
    public void Activate_Gameplay(bool _value) => gamePlayPanel.SetActive(_value);
    public void Activate_HowToPlay(bool _value) => howToPlayPanel.SetActive(_value);
    public void Activate_PlayerOrBot(bool _value) => playerOrBotPanel.SetActive(_value);

    public void DeactivateAll()
    {
        selectMenuPanel.SetActive(false);
        difficultyPanel.SetActive(false);
        gamePlayPanel.SetActive(false);
        playerOrBotPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
    }
}
