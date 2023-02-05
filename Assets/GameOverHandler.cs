using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverHandler : MonoBehaviour
{
    [SerializeField] Hitpoints HitpointsForGameover;
    [SerializeField] GameObject GameOverScreen;
    [SerializeField] GameObject RestartHintScreen;
    [SerializeField] GameObject IngameUI;

    public event Action OnGameOver;
    // Start is called before the first frame update
    void Start()
    {
        HitpointsForGameover.OnDeath += GameOver;
    }

    private void GameOver()
    {
        GameOverScreen.SetActive(true);
        IngameUI.SetActive(false);

        //TODO: Show after short time
        ShowRestartScreen();

        OnGameOver?.Invoke();
    }

    private void ShowRestartScreen()
    {
        RestartHintScreen.SetActive(true);
    }

    public void Restart()
    {
        Debug.Log("Restart");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
