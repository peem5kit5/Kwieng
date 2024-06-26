using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

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
    [SerializeField] private GameObject winnerPanel;
    [SerializeField] private TextMeshProUGUI winnerName;
    public TextMeshProUGUI WinnerDateTime;

    [Header("Abilities UI")]
    [SerializeField] private UI_Abilities[] AuntAbilities;
    [SerializeField] private UI_Abilities[] SoldierAbilities;

    [Header("Google")]
    public Image GooglePhoto;
    public TextMeshProUGUI UserNameText;
    public TextMeshProUGUI EmailText;

    private void Start()
    {
        Canvas.sortingLayerName = "Blocking";

        selectMenuPanel.SetActive(true);
        gamePlayPanel.SetActive(false);
        difficultyPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
        winnerPanel.SetActive(false);
    }

    public void Init()
    {
        foreach (var _abilities in AuntAbilities)
            _abilities.SetEntity(GameObject.Find("Aunt").GetComponent<Entity>());

        foreach (var _abilities in SoldierAbilities)
            _abilities.SetEntity(GameObject.Find("SoldierPig").GetComponent<Entity>());
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!difficultyPanel)
            difficultyPanel = GameObject.Find("DifficultyPanel");

        if (!gamePlayPanel)
            gamePlayPanel = GameObject.Find("GamePlayPanel");

        if (!howToPlayPanel)
            howToPlayPanel = GameObject.Find("HowToPlayPanel");

    }
#endif

    public void Activate_Menu(bool _value) => selectMenuPanel.SetActive(_value);
    public void Activate_Difficulty(bool _value) => difficultyPanel.SetActive(_value);
    public void Activate_Gameplay(bool _value) => gamePlayPanel.SetActive(_value);
    public void Activate_HowToPlay(bool _value) => howToPlayPanel.SetActive(_value);
    public void Activate_PlayerOrBot(bool _value) => playerOrBotPanel.SetActive(_value);
    public void Activate_Winner(bool _value, string _winnerName)
    {
        winnerPanel.SetActive(_value);
        winnerName.text = _winnerName;

        DateTime _dt = DateTime.Now;
        WinnerDateTime.text = string.Format("{0}/{1}/{2}", _dt.Month, _dt.Day, _dt.Year);

    }

    public void SelectEasyDifficulty()
    {
        var _csv = CSVDataLoader.Instance;
        GameManagers.Instance.SetDifficultyButton(_csv.AttributeDataDict["EasyEnemyHP"]);
        GameManagers.Instance.InitGame();
    }

    public void SelectNormalDifficulty()
    {
        var _csv = CSVDataLoader.Instance;
        GameManagers.Instance.SetDifficultyButton(_csv.AttributeDataDict["NormalEnemyHP"]);
        GameManagers.Instance.InitGame();
    }

    public void SelectHardDifficulty()
    {
        var _csv = CSVDataLoader.Instance;
        GameManagers.Instance.SetDifficultyButton(_csv.AttributeDataDict["HardEnemyHP"]);
        GameManagers.Instance.InitGame();
    }

    public void DeactivateAll()
    {
        selectMenuPanel.SetActive(false);
        difficultyPanel.SetActive(false);
        gamePlayPanel.SetActive(false);
        playerOrBotPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
        winnerPanel.SetActive(false);
    }
}
