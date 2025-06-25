using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public static ScoreManager Instance;

    private int totalScore = 0;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        UpdateScoreText();
    }
    public void AddScore(int amount)
    {
        totalScore += amount;
        UpdateScoreText();
    }

    public int GetScore()
    {
        return totalScore;
    }
    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + totalScore.ToString();
    }
}