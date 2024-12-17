using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform _arCamera;
    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private GameState _gameState;

    private GameObject _currentBall;
    private Vector2 _touchInitialPosition, _touchFinalPosition;
    private float _ySwipeDelta;

    private void Awake()
    {
        // listen to Enter Ball Setup Event
        _gameState.OnEnterBallSetup.AddListener(EnableSelf);

        // Resets game state to the needed values for a new game
        _gameState.ResetState();

        _gameState.CurrentGameState = GameState.GameStateEnum.PlacingPinDeckAndLane;

    }

    private void OnEnable()
    {
        BallInitialSetup();
    }

    private void OnDisable()
    {
        _gameState.OnEnterBallSetup.RemoveListener(EnableSelf);
    }

    void EnableSelf()
    {
        enabled = true;
    }

    private void BallInitialSetup()
    {
        // instantiate ball
        _currentBall = Instantiate(_ballPrefab, new Vector3(0, 1000, 0), Quaternion.identity);

        // switch to ReadyToThrow state
        _gameState.CurrentGameState = GameState.GameStateEnum.ReadyToThrow;
    }

    // Detects screen swipe and calls ThrowBall
    void DetectScreenSwipe()
    {
        foreach (var touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                _touchInitialPosition = touch.position;
            }
            if (touch.phase == TouchPhase.Ended)
            {
                _touchFinalPosition = touch.position;

                if (_touchFinalPosition.y > _touchInitialPosition.y)
                {
                    _ySwipeDelta = _touchFinalPosition.y - _touchInitialPosition.y;
                }

                ThrowBall();
            }
        }
    }

    // Gets a ball and sets position, rotation and adds force to it
    void ThrowBall()
    {
        // enable gravity
        _currentBall.GetComponent<Rigidbody>().useGravity = true;

        // store force multiplier
        float throwPowerMultiplier = 0.05f;

        // store ar camera rotation
        Quaternion lookRotation = _arCamera.rotation;

#if UNITY_EDITOR
        // store camera and mouse position and convert to a world direction
        Camera cam = _arCamera.GetComponent<Camera>();
        Vector3 mousePos = Input.mousePosition;
        Vector3 mouseDir = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.farClipPlane));

        // store rotation direction
        lookRotation = Quaternion.LookRotation(mouseDir, Vector3.up);

        // override swipe and power for editor only
        _ySwipeDelta = 1.5f;
        throwPowerMultiplier = 60.00f;
#endif

        // set start ball position facing ar camera
        _currentBall.transform.position = _arCamera.position;
        _currentBall.transform.rotation = lookRotation;

        // calculate force and apply to the ball's rigidbody
        Vector3 forceVector = _currentBall.transform.forward * (_ySwipeDelta * throwPowerMultiplier);
        _currentBall.GetComponent<Rigidbody>().AddForce(forceVector, ForceMode.Impulse);

        // update balls remaining
        _gameState.RemainingBalls--;

        // set ball in play state
        _gameState.CurrentGameState = GameState.GameStateEnum.BallInPlay;
    }

    // Update is called once per frame
    void Update()
    {
        switch (_gameState.CurrentGameState)
        {
            case GameState.GameStateEnum.ReadyToThrow:
                // track touch to throw, device only
                DetectScreenSwipe();

#if UNITY_EDITOR
                // desktop editor only, track mouse button to throw
                if (Input.GetMouseButtonDown(1)) ThrowBall();
#endif
               break;

            case GameState.GameStateEnum.BallInPlay:
                // if the ball falls below -20, you have ended the play
                if (_currentBall.transform.position.y < -20)
                {
                    Debug.Log("PLAYER PLAY END!");

                    // reset ball
                    _currentBall.transform.position = new Vector3(0, 1000, 0);
                    _currentBall.transform.rotation = Quaternion.identity;

                    // reset rigidbody force
                    Rigidbody rb = _currentBall.GetComponent<Rigidbody>();
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    rb.useGravity = false;

                    // ball has been thrown, set ball play end state
                    _gameState.CurrentGameState = GameState.GameStateEnum.BallPlayEnd;
                }
               break;
        }
    }
}
