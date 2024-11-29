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
    private GameObject _pinDeckClone;
    private Transform _pinDeckSpawnPoint;
    private bool _pinDeckCreated = false;

    public void CreatePinDeck()
    {
        StartCoroutine(SetupBowlingLaneRoutine());
    }

    private IEnumerator SetupBowlingLaneRoutine()
    {
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        // For testing purposes, start and create pin deck on mouse click
#if UNITY_EDITOR
        if (!_pinDeckCreated)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _pinDeckCreated = true;
                CreatePinDeck();
                Debug.Log("Mouse Left Button Clicked, CreatePinDeck()");
            }
        }
#endif
    }
}
