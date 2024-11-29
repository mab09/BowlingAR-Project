using System.Collections;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameState _gameState;
    [SerializeField] private TMP_Text _scoreUI;

    [SerializeField] private GameObject _nextTurnPanel;
    [SerializeField] private GameObject _strikePanel;
    [SerializeField] private GameObject _placePinDeckPanel;
    [SerializeField] private GameObject _controlsPanel_1;
    [SerializeField] private GameObject _controlsPanel_2;
    [SerializeField] private float _turnWaitTime = 3;

    public void ShowNextTurnUI()
    {
        // hide strike text
        _strikePanel.SetActive(false);

        StartCoroutine(ShowNextTurnRoutine());
    }

    private IEnumerator ShowNextTurnRoutine()
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

    void UpdateScoreUI(int newScore)
    {
        _scoreUI.text = $"{newScore}";
    }


}
