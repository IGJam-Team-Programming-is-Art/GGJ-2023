using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPlaceBuilding : MonoBehaviour
{
    [SerializeField] GameObject BuildingPrefab;
    [SerializeField] float Cooldown;

    private float _cooldownEndTimestamp;

    public void OnBuildTargeted(InputAction.CallbackContext context)
    {
        var screenPos = Mouse.current.position.ReadValue();
        var target = Extensions.GetGroundPoint(screenPos);
        Build(target, BuildingPrefab);
    }

    private void Build(Vector3 targetPoint, GameObject BuildingPrefab)
    {
        if (_cooldownEndTimestamp > Time.time)
        {
            return;
        }
        _cooldownEndTimestamp = Time.time + Cooldown;

        Instantiate(BuildingPrefab, targetPoint, Quaternion.identity);
    }
}