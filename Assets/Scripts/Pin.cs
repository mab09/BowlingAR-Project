using UnityEngine;

public class Pin : MonoBehaviour
{
    [SerializeField] private GameState _gameState;

    private Rigidbody _rb;
    private MeshCollider _collider;

    private Vector3 _originalPosition;
    private Quaternion _originalRotation;

    private float _moveSpeed = 1.5f;
    private Vector3 _raisedPosition = new Vector3(0, 0.85f, 0);

    private void Awake()
    {
        // store rigidbody and collider refs
        _rb = transform.GetComponent<Rigidbody>();
        _collider = transform.GetComponent<MeshCollider>();

        // store original position and rotation
        _originalPosition = transform.position;
        _originalRotation = transform.rotation;

        // initialize pin transform and rotation
        Reset();
    }

    public void Reset()
    {
        // restore original position and rotation
        transform.position = _originalPosition;
        transform.rotation = _originalRotation;

        // set initial raised local position 
        transform.localPosition += _raisedPosition;

        // stop movements
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
    }
}
