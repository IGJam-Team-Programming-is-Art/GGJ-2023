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
    [SerializeField] Hitpoints HitPointsReference;

    [Inject] PlayerReferences _playerReferences;

    private void Start()
    {
        if (HitPointsReference == null)
        {
            HitPointsReference = _playerReferences.Hitpoints;
        }
        Slider.minValue = 0;
        Slider.maxValue = HitPointsReference.Max;
        Slider.value = HitPointsReference.Current;
        if (HitPointText)
        {
            HitPointText.text = GetHitpointText();
        }

        HitPointsReference.OnModify += OnModify;
        HitPointsReference.OnDeath += OnDeath;
    }

    private void OnModify(int amount)
    {
        Slider.value = HitPointsReference.Current;
        if (HitPointText)
        {
            HitPointText.text = GetHitpointText();
        }
    }

    private void OnDeath()
    {
        if (HitPointText)
        {
            HitPointText.text = "Dead";
        }
    }

    private string GetHitpointText() => $"{HitPointsReference.Current} /  {HitPointsReference.Max}";

}
