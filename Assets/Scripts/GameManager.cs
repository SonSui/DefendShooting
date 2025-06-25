using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int score;
    public int playerMaxLife = 1;
    private int playerLife = 1;
    private GameObject playerInstance;
    public GameObject gameOverText;
    public GameObject gameOverUI;

    public float gameOverDelay = 3f;
    private float timer = 3f;

    public static GameManager instance;
    public enum GameState
    {
        Title,
        Game,
        GameOver,
        ShowingScore,
        Result
    };
    public GameState gameState = GameState.Title;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        Application.targetFrameRate = 60;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }
    private void OnSceneLoad(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == "TitleScene")
        {
            gameState = GameState.Title;
            score = 0;
            playerLife = playerMaxLife;
        }
        else if (scene.name == "GameScene")
        {
            gameState = GameState.Game;
            timer = gameOverDelay;
        }
        else if (scene.name == "ResultScene")
        {
            gameState = GameState.ShowingScore;
            GameObject btnGO = Instantiate(gameOverUI);
        }
    }
    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.A) || (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame)))
        {
            if (gameState == GameState.Title) { SFXManager.instance?.PlayButtonSound(); SceneManager.LoadScene("GameScene"); }
            else if (gameState == GameState.GameOver) { SFXManager.instance?.PlayButtonSound(); SceneManager.LoadScene("ResultScene"); }
            else if (gameState == GameState.Result) { SFXManager.instance?.PlayButtonSound(); SceneManager.LoadScene("TitleScene"); }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if(gameState == GameState.GameOver)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                GoToResult();
            }
        }
    }
    public void AddScore(int sc)
    {
        score += sc;
        SFXManager.instance?.PlayScoreSound();
    }
    public int GetScore()
    {
        return score;
    }
    public void GameOver()
    {
        gameState = GameState.GameOver;
        Instantiate(gameOverText);
        if(ScoreManager.Instance != null)
        {
            score = ScoreManager.Instance.GetScore();
        }
    }
    public void ScoreOver() { gameState = GameState.Result; }
    public void PlayerDead()
    {
        GameOver();
    }

    public void GoToResult()
    {
        SceneManager.LoadScene("ResultScene");
    }

    public void OnStartButtonDown()
    {
        if (gameState == GameState.Title)
        {
            SFXManager.instance?.PlayButtonSound();
            SceneManager.LoadScene("GameScene");
        }
        else if (gameState == GameState.GameOver)
        {
            SFXManager.instance?.PlayButtonSound();
            SceneManager.LoadScene("ResultScene");
        }
        else if (gameState == GameState.ShowingScore)
        {
            SFXManager.instance?.PlayButtonSound();
            SceneManager.LoadScene("TitleScene");
        }
    }
}
