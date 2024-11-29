using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GameState", menuName = "ScriptableObjects/CreateGameStateAsset")]
public class GameState : ScriptableObject
{
    public enum GameStateEnum
    {
        TitleScreen,
        PlacingPinDeckAndLane,
        SetupBalls,
        ReadyToThrow,
        BallInPlay,
        BallPlayEnd,
        StrikeAchieved,
        TurnEnd,
        ResettingDeck,
        GameEnded
    }

    [SerializeField] private GameStateEnum _currentGameState;
    public GameStateEnum CurrentGameState
    {
        get => _currentGameState;
        set => _currentGameState = value;
    }

    [SerializeField] private int _score = 0;
    [SerializeField] private int _remainingBalls = 0;
    [SerializeField] private int _currentTurn = 0;
    [SerializeField] private int _maxTurns = 5;
    [SerializeField] private int _strikeCounter = 0;
    [SerializeField] private int _strikeExtraPoints = 10;
    [SerializeField] private float _throwPowerMultiplier = 0.05f;
    [HideInInspector] public UnityEvent<int> OnScoreChanged;

    public int Score
    {
        get => _score;
        set
        {
            _score = value;
            OnScoreChanged?.Invoke(_score);
        }
    }
    public int RemainingBalls
    {
        get => _remainingBalls;
        set => _remainingBalls = value;
    }
    public int CurrentTurn
    {
        get => _currentTurn;
        set => _currentTurn = value;
    }
    public int StrikeCounter
    {
        get => _strikeCounter;
        set => _strikeCounter = value;
    }
    public int MaxTurns
    {
        get => _maxTurns;
        set => _maxTurns = value;
    }
    public int StrikeExtraPoints
    {
        get => _strikeExtraPoints;
        set => _strikeExtraPoints = value;
    }
    public float ThrowPowerMultiplier
    {
        get => _throwPowerMultiplier;
        set => _throwPowerMultiplier = value;
    }

    public void ResetState()
    {
        _currentTurn = 1;
        _score = 0;
        _remainingBalls = MaxTurns;
    }

}
