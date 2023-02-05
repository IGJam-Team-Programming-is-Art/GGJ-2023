using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

public class PlayerPlaceBuilding : MonoBehaviour
{
    [SerializeField] private GameObject BuildingPrefab;
    [SerializeField] private float Cooldown;
    [SerializeField] [Inject] private GameOverHandler _gameOverHandler;
    [Inject] private CameraReference _cameraReference;

    private float _cooldownEndTimestamp;

    private void Awake()
    {
        if (_gameOverHandler != null) _gameOverHandler.OnGameOver += OnGameOver;
    }

    public void OnBuildTargeted(InputAction.CallbackContext context)
    {
        var screenPos = Mouse.current.position.ReadValue();
        var target = Extensions.GetGroundPoint(screenPos);
        Build(target, BuildingPrefab);
    }

    private void OnGameOver()
    {
        enabled = false;
    }


    private void Build(Vector3 targetPoint, GameObject BuildingPrefab)
    {
        if (_cooldownEndTimestamp > Time.time) return;
        _cooldownEndTimestamp = Time.time + Cooldown;

        var building = Instantiate(BuildingPrefab, targetPoint, Quaternion.identity);
        building.GetComponentInChildren<HitpointUi>().CameraReference = _cameraReference;
    }
}