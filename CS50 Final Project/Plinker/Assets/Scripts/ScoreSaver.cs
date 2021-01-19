using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSaver : MonoBehaviour
{
    public Text ScoreText;
    public Text HighScoreText;
    public Text HeadshotText;
    private int Level;

    private int HighScore;
    private int Score;
    private int HeadShots;
    private int Targets;

    private string LevelString;

    public GameObject ScoreH;
    public ScoreHolder ScoreHolder;

    void Start() 
    {
        ScoreH = GameObject.Find("ScoreHolder");
        ScoreHolder = ScoreH.GetComponent<ScoreHolder>();
        ScoreHolder.GetScores(out Score, out HeadShots, out Targets, out Level);

        LevelString = "HighScore" + Level;
        HighScore = PlayerPrefs.GetInt(LevelString);
        CheckHighscore();
        ScoreText.text = Score.ToString();
        HighScoreText.text = HighScore.ToString();

        HeadshotText.text = HeadShots.ToString() + "/" + Targets.ToString(); 

        Destroy(ScoreH);
    }

    void CheckHighscore()
    {
        if (Score > HighScore)
        {
            HighScore = Score;
            PlayerPrefs.SetInt(LevelString, Score);
        }
    }

    public void ResetScore()
    {
        HighScoreText.text = "0";
        PlayerPrefs.SetInt(LevelString, 0);
    }


}
