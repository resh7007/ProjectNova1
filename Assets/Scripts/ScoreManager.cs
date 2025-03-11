using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public TextMeshProUGUI scoreText;
    private int score = 0;

    private void Awake()
    {
        Instance = this;
        LoadProgress();
    }

    public void UpdateScore(int points)
    {
        score += points;
        scoreText.text = "Score: " + score;
        SaveProgress();
    }

    private void SaveProgress()
    {
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.Save();
    }

    private void LoadProgress()
    {
        if (PlayerPrefs.HasKey("Score"))
        {
            score = PlayerPrefs.GetInt("Score");
            scoreText.text = "Score: " + score;
        }
    }
}