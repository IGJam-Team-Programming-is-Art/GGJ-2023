using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class HitpointUi : MonoBehaviour
{
    [SerializeField] Slider Slider;
    [SerializeField] TMP_Text HitPointText;

    [Inject] PlayerReferences _playerReferences;

    private void Start()
    {
        Slider.minValue = 0;
        Slider.maxValue = _playerReferences.Hitpoints.Max;
        Slider.value = _playerReferences.Hitpoints.Current;
        HitPointText.text = GetHitpointText();

        _playerReferences.Hitpoints.OnModify += OnModify;
        _playerReferences.Hitpoints.OnDeath += OnDeath;
    }

    private void OnModify(int amount)
    {
        Slider.value = _playerReferences.Hitpoints.Current;
    }

    private void OnDeath()
    {
        HitPointText.text = "Dead";
    }

    private string GetHitpointText() => $"{_playerReferences.Hitpoints.Current} /  {_playerReferences.Hitpoints.Max}";

}
