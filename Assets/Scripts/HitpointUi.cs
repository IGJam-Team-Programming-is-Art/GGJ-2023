using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class HitpointUi : MonoBehaviour
{
    [SerializeField] private bool _rotateTowardsPlayer = false;

    private Transform _canvas;
    [SerializeField] private Slider Slider;
    [SerializeField] private TMP_Text HitPointText;
    [SerializeField] private Hitpoints HitPointsReference;

    [Inject] private PlayerReferences _playerReferences;
    public CameraReference CameraReference;

    private void Start()
    {
        _canvas = GetComponentInParent<Transform>();

        if (HitPointsReference == null) HitPointsReference = _playerReferences.Hitpoints;
        Slider.minValue = 0;
        Slider.maxValue = HitPointsReference.Max;
        Slider.value = HitPointsReference.Current;
        if (HitPointText) HitPointText.text = GetHitpointText();

        HitPointsReference.OnModify += OnModify;
        HitPointsReference.OnDeath += OnDeath;
    }

    private void LateUpdate()
    {
        if (!_rotateTowardsPlayer) return;

        var rotation = CameraReference.Transform.rotation;
        transform.LookAt(transform.position + rotation * Vector3.forward,
            rotation * Vector3.up);
    }

    private void OnModify(int amount)
    {
        Slider.value = HitPointsReference.Current;
        if (HitPointText) HitPointText.text = GetHitpointText();
    }

    private void OnDeath()
    {
        if (HitPointText) HitPointText.text = "Dead";
    }

    private string GetHitpointText()
    {
        return $"{HitPointsReference.Current} /  {HitPointsReference.Max}";
    }
}