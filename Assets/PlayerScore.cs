using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public int Score;
    public event Action<int> OnScoreChange;

    public void IncreaseScore(int points)
    {
        Score += points;
        OnScoreChange.Invoke(Score);
    }
}
