
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManagers : Singleton<GameManagers>
{
    [Header("References")]
    public TapShooter TapShooter;
    [SerializeField] private LoginController loginController;
    [SerializeField] private Timer timer;
    [SerializeField] private UI_Controller uiController;
    [SerializeField] private Slider windForceSlider;
    [SerializeField] private TextMeshProUGUI turnStatusText;

    [Header("Aunt")]
    [SerializeField] private AttributeData auntAttributeData;
    [SerializeField] private GameObject aunt;

    [Header("Soldier")]
    [SerializeField] private AttributeData soldierAttributeData;
    [SerializeField] private GameObject soldierPig;

    [Header("Projectile")]
    public GameObject SoldierProjectile;
    public GameObject AuntProjectile;

    [Header("State")]
    public State GameState;

    [Header("Wind")]
    public float WindForce = 0;

    public enum State
    {
        VsBot,
        VsPlayer
    }

    [Header("Turn")]
    public Turn CurrentTurn;

    public enum Turn
    {
        TurnPlayer1,
        TurnPlayer2
    }

    public GameObject Aunt => aunt;
    public GameObject Soldier => soldierPig;
    private HashSet<Entity> entityList = new HashSet<Entity>();

    public bool IsGameFinished;

#if Unity_Editor
    private void OnValidate()
    {
        if (!difficultyPanel)
            difficultyPanel = GameObject.Find("DifficultyPanel");

        if (!hardDifficulty)
            hardDifficulty = GameObject.Find("HardDifficultyButton").GetComponent<Button>();

        if (!normalDifficulty)
            normalDifficulty = GameObject.Find("NormalDifficultyButton").GetComponent<Button>();

        if (!easyDifficulty)
            easyDifficulty = GameObject.Find("EasyDifficultyButton").GetComponent<Button>();

        if (!uiController)
            turnHandler = FindObjectOfType<UI_Controller>();

        if (!tapShooter)
            tapShooter = FindObjectOfType<TapShooter>();
    }
#endif

    private void Start()
    {
        loginController = FindObjectOfType<LoginController>();
        if (loginController)
        {
            if (loginController.IsHavingGoogle)
            {
                loginController.LoadImage(uiController.GooglePhoto);
                uiController.UserNameText.text = loginController.UserName;
                uiController.EmailText.text = loginController.Email;
            }
        }
    }

    public void InitGameState_VsPlayer()
    {
        GameState = State.VsPlayer;
        InitGame();
    }

    public void InitGameState_VsBot()
    {
        GameState = State.VsBot;
        SelectDifficulty();
    }

    private void SelectDifficulty()
    {
        if (GameState != State.VsBot)
            return;

        var _csv = CSVDataLoader.Instance;
        auntAttributeData = _csv.AttributeDataDict["PlayerHP"];
    }

    public void SetDifficultyButton(AttributeData _attributeData) => soldierAttributeData = _attributeData;

    public void InitGame()
    {
        uiController.DeactivateAll();
        uiController.Activate_Gameplay(true);

        var _player1 = aunt.AddComponent<Player>();
        _player1.SetPlayer(Player.PlayerState.Player1);
        _player1.AttributeData = CSVDataLoader.Instance.AttributeDataDict["PlayerHP"];
        _player1.Init();

        entityList.Add(_player1);

        if(GameState == State.VsBot)
        {
            var _botBehaviour = soldierPig.AddComponent<BotBehaviour>();

            _botBehaviour.SetAttribute(soldierAttributeData);
            _botBehaviour.Init();

            entityList.Add(_botBehaviour);
        }
        else
        {
            var _player2 = soldierPig.AddComponent<Player>();

            _player2.SetPlayer(Player.PlayerState.Player2);
            _player2.AttributeData = CSVDataLoader.Instance.AttributeDataDict["PlayerHP"];
            _player2.Init();

            entityList.Add(_player2);
        }

        uiController.Canvas.sortingLayerName = "Default";
        uiController.Init();

        timer.Init();

        TapShooter.IsPlaying = true;
        TapShooter.gameObject.SetActive(true);
        EndTurn();
    }

    public void EndTurn()
    {
        WindForce = Random.Range(-5, 5);
        windForceSlider.SetValueWithoutNotify(WindForce);

        if (CurrentTurn == Turn.TurnPlayer1)
        {
            CurrentTurn = Turn.TurnPlayer2;
            turnStatusText.text = "<= Left Side Turn.";
        }
        else
        {
            CurrentTurn = Turn.TurnPlayer1;
            turnStatusText.text = "Right Side Turn. =>";
        }

        foreach (var _entity in entityList)
        {
            _entity.Turn();

            if(_entity.IsTheirTurn)
                TapShooter.ValidateEntity(_entity);
        }

        TapShooter.IsBot = false;
        TapShooter.Shooted = false;
        timer.SetMaxTime();
    }

    public void ConclusionGame(string _loserName)
    {
        var _auntEntity = aunt.GetComponent<Entity>();
        var _soldierEntity = soldierPig.GetComponent<Entity>();

        if (_loserName == "Aunt")
        {
            uiController.Activate_Winner(true, "Soldier Win!");

            _auntEntity.SetAnimationLoop("Moody UnFriendly");
            _soldierEntity.SetAnimationLoop("Happy Friendly");

        }
        else
        {
            uiController.Activate_Winner(true, "Aunt Win!");

            _auntEntity.SetAnimationLoop("Happy UnFriendly");
            _soldierEntity.SetAnimationLoop("Moody Friendly");
        }

        TapShooter.gameObject.SetActive(false);
        IsGameFinished = true;
    }
}
