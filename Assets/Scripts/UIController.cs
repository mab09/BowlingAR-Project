using System.Collections;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameState _gameState;
    [SerializeField] private TMP_Text _scoreUI;
    [SerializeField] private TMP_Text _remainingBallsUI;
    [SerializeField] private TMP_Text _finalScoreUI;

    [SerializeField] private GameObject _nextTurnPanel;
    [SerializeField] private GameObject _strikePanel;
    [SerializeField] private GameObject _placePinDeckPanel;
    [SerializeField] private GameObject _controlsPanel_1;
    [SerializeField] private GameObject _controlsPanel_2;
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private float _turnWaitTime = 3;

    private bool _throwInstructionShowed = false;

    private void OnEnable()
    {
        _gameState.OnEnterBallSetup.AddListener(HidePlaceInDeckPanel);

        _gameState.OnScoreChanged.AddListener(UpdateScoreUI);
        _gameState.OnBallInPlay.AddListener(UpdateAmountOfBallsUI);
        _gameState.OnTurnEnd.AddListener(ShowNextTurnUI);
        _gameState.OnGameEnded.AddListener(ShowGameOverScreen);
        _gameState.OnStrikeAchieved.AddListener(ShowStrikeUI);
    }

    private void OnDisable()
    {
        _gameState.OnEnterBallSetup.RemoveListener(HidePlaceInDeckPanel);

        _gameState.OnScoreChanged.RemoveListener(UpdateScoreUI);
        _gameState.OnBallInPlay.RemoveListener(UpdateAmountOfBallsUI);
        _gameState.OnTurnEnd.RemoveListener(ShowNextTurnUI);
        _gameState.OnGameEnded.RemoveListener(ShowGameOverScreen);
        _gameState.OnStrikeAchieved.RemoveListener(ShowStrikeUI);
    }

    void HidePlaceInDeckPanel()
    {
        // hide place pin panel and show control instructions
        _placePinDeckPanel.SetActive(false);

        ShowControls();
    }

    void ShowControls()
    {
        if (_throwInstructionShowed) return;

        _throwInstructionShowed = true;
        _controlsPanel_1.SetActive(true);

        Invoke("HideControls_1", 3);
    }

    void HideControls_1()
    {
        _controlsPanel_1.SetActive(false);
        _controlsPanel_2.SetActive(true);

        Invoke("HideControls_2", 3);
    }

    void HideControls_2()
    {
        _controlsPanel_2.SetActive(false);
    }

    void UpdateScoreUI(int newScore)
    {
        _scoreUI.text = $"{newScore}";
    }
    void UpdateAmountOfBallsUI()
    {
        _remainingBallsUI.text = $"{_gameState.RemainingBalls}";
    }
    public void ShowStrikeUI()
    {
        // display strike text
        _strikePanel.SetActive(true);
    }

    public void ShowNextTurnUI()
    {
        // hide strike text
        _strikePanel.SetActive(false);

        StartCoroutine(ShowNextTurnRoutine());
    }

    IEnumerator ShowNextTurnRoutine()
    {
        Debug.Log("SHOW NEXT TURN");

        // Increases the current turn number
        _gameState.CurrentTurn++;

        if (_gameState.CurrentTurn <= _gameState.MaxTurns)
        {
            _nextTurnPanel.SetActive(true);
            _nextTurnPanel.GetComponentInChildren<TMP_Text>().text = $"Turn {_gameState.CurrentTurn}";

            yield return new WaitForSeconds(_turnWaitTime);

            _nextTurnPanel.SetActive(false);
            _gameState.CurrentGameState = GameState.GameStateEnum.ResettingDeck;
        }
        else
        {
            _gameState.CurrentGameState = GameState.GameStateEnum.GameEnded;
        }
    }

    public void ShowGameOverScreen()
    {
        _strikePanel.SetActive(false);

        _gameOverScreen.SetActive(true);
        _finalScoreUI.text = $"{_gameState.Score}";
    }

    void Start()
    {
        // show place pin panel
        _placePinDeckPanel.SetActive(true);

        // update HUD score and balls
        UpdateAmountOfBallsUI();
        UpdateScoreUI(0);
    }
}
