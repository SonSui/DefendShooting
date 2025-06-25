using UnityEngine;
using TMPro;

public class ResultUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private void Start()
    {
        if (GameManager.instance != null)
        {
            scoreText.text = GameManager.instance.score.ToString();
        }
        else
        {
            scoreText.text = "0";
        }
    }
    public void OnBackToTitleButtonClicked()
    {
        GameManager.instance?.OnStartButtonDown();
    }
}
