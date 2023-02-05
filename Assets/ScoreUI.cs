using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] PlayerScore Score;
    [SerializeField] TMP_Text ScoreText;
    // Start is called before the first frame update
    void Start()
    {
        ScoreText.text = $"Score: {Score.Score}";
        Score.OnScoreChange += OnScoreChange;
    }

    private void OnScoreChange(int score)
    {
        ScoreText.text = $"Score: {Score.Score}";
    }
}
