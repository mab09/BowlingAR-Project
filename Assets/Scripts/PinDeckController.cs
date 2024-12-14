using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Vuforia;

public class PinDeckController : MonoBehaviour
{
    [SerializeField] private Transform _arCamera;
    [SerializeField] private GameObject _bowlingLanePrefab;
    [SerializeField] private GameObject _pinDeckPrefab;
    [SerializeField] private PlaneFinderBehaviour _planeFinder;
    [SerializeField] private GameState _gameState;

    private GameObject _pinDeckClone;
    private Transform _pinDeckSpawnPoint;
    private bool _pinDeckCreated = false;
    private int _downedPins = 0;

    private Pin[] _pins;

    void OnEnable()
    {
        _gameState.OnBallPlayEnd.AddListener(StartBallPlayEnded);
        _gameState.OnResettingDeck.AddListener(StartPlaceNewDeckOnLane);
    }

    void OnDisable()
    {
        _gameState.OnBallPlayEnd.RemoveListener(StartBallPlayEnded);
        _gameState.OnResettingDeck.RemoveListener(StartPlaceNewDeckOnLane);
    }

    private void Awake()
    {
        // set ar camera position for editor testing
#if UNITY_EDITOR
        _arCamera.transform.position = new Vector3(0, 1.4f, 4);
        _arCamera.transform.eulerAngles = new Vector3(
            _arCamera.transform.eulerAngles.x,
            _arCamera.transform.eulerAngles.y + 180,
            _arCamera.transform.eulerAngles.z
        );
#endif
    }

    // Called from content positioning behavior (a Vuforia component inside Plane Finder)
    public void CreatePinDeck()
    {
        StartCoroutine(SetupBowlingLaneRoutine());
    }

    private IEnumerator SetupBowlingLaneRoutine()
    {
        // Get plane indicator's transform
        Transform defaultPlaneIndicator = _planeFinder.PlaneIndicator.transform;

        // Gets camera direction for rotation
        Vector3 directionTowardsCamera = defaultPlaneIndicator.position + _arCamera.position;

        // invert direction for editor testing 
#if UNITY_EDITOR
        directionTowardsCamera = defaultPlaneIndicator.position - _arCamera.position;
#endif

        // Reset the Y position to keep the lane straight
        directionTowardsCamera.y = 0;

        // Store rotation of the track to face ARCamera
        Quaternion lookRotation = Quaternion.LookRotation(-directionTowardsCamera, Vector3.up);

        // Create a new pin deck base
        GameObject bowlingLaneClone = Instantiate(_bowlingLanePrefab, defaultPlaneIndicator.position, lookRotation);

        // Get position and rotation for new pin deck from the spawn point
        _pinDeckSpawnPoint = bowlingLaneClone.transform.Find("PinDeckSpawnPoint");

        // Creates a new pin deck
        _pinDeckClone = Instantiate(_pinDeckPrefab, _pinDeckSpawnPoint.position, _pinDeckSpawnPoint.rotation);

        yield return new WaitForSeconds(1);

        _gameState.CurrentGameState = GameState.GameStateEnum.SetupBalls;

        _pins = _pinDeckClone.transform.GetComponentsInChildren<Pin>();

        LowerPinDeck();

        yield return new WaitForSeconds(1);
    }

    void StartBallPlayEnded()
    {
        Debug.Log("BallPlayEnded()");

        StartCoroutine(BallPlayEnded());
    }

    IEnumerator BallPlayEnded()
    {
        foreach (Pin pin in _pins)
        {
            if (pin.IsPinDown())
            {
                _gameState.Score++;

                _gameState.StrikeCounter++;

                _downedPins++;
            }
        }

        if (_gameState.StrikeCounter == 10)
        {
            _gameState.CurrentGameState = GameState.GameStateEnum.StrikeAchieved;

            _gameState.Score += _gameState.StrikeExtraPoints;

            yield return new WaitForSeconds(2);
        }

        _gameState.StrikeCounter = 0;

        RaisePinDeck();

        yield return new WaitForSeconds(2);

        _gameState.CurrentGameState = GameState.GameStateEnum.TurnEnd;

    }
    void StartPlaceNewDeckOnLane()
    {
        Debug.Log("PLACE NEW DECK ON LANE");

        StartCoroutine(PlaceNewDeckOnLane());
    }

    IEnumerator PlaceNewDeckOnLane()
    {
        foreach (Pin pin in _pins)
        {
            if (pin.IsPinDown())
            {
                pin.DisablePin();
            }

            if(_downedPins == 10)
            {
                pin.EnablePin();
            }
            
            pin.Reset();
            pin.StartLowerPin();
        }

        if(_downedPins >= 10) _downedPins = 0;

        yield return new WaitForSeconds(2);

        _gameState.CurrentGameState = GameState.GameStateEnum.ReadyToThrow;
    }
    void LowerPinDeck()
    {
        foreach (Pin pin in _pins)
        {
            if (!pin.IsPinDown()) pin.StartLowerPin();
        }
    }
    void RaisePinDeck()
    {
        foreach (Pin pin in _pins)
        {
             pin.StartRaisePin();
        }
    }

}
