using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [SerializeField]
    private TextMeshProUGUI[] currentScoreTexts;
    [SerializeField]
    private TextMeshProUGUI[] highScoreTexts;

    private int currentScore;
    private int highScore;
    

    // Start is called before the first frame update
    void Awake() {
        Instance = this;
        foreach(TextMeshProUGUI currentScoreText in currentScoreTexts) {
            currentScoreText.text = currentScore.ToString("D7");
        } 
        
        highScore = PlayerPrefs.GetInt("Highscore", 0);
        foreach(TextMeshProUGUI highScoreText in highScoreTexts) {
            highScoreText.text = highScore.ToString("D7");
        }
        
    }

    public void AddScorePoint(int scorePoints) {
        currentScore += scorePoints;
        foreach(TextMeshProUGUI currentScoreText in currentScoreTexts) {
            currentScoreText.text = currentScore.ToString("D7");
        } 

        if(highScore < currentScore) {
            PlayerPrefs.SetInt("Highscore", currentScore);
        }
    }
}
