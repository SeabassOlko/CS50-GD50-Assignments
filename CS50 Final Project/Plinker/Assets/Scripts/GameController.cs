using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public GameObject ScoreH;

    private ScoreHolder ScoreHolder;

    [SerializeField]
    public int Score;
    [SerializeField]
    private int Headshots;

    [SerializeField]
    public int TargetAmount;
    private int LevelTargetAmount;

    private int Level;

    [SerializeField]
    public float MapWidth;

    public AudioSource _HeadShot;
    public AudioSource _BodyShot;

    private void Start() 
    {
        Score = 0;
        Headshots = 0;

        ScoreHolder = ScoreH.GetComponent<ScoreHolder>();

        Scene scene = SceneManager.GetActiveScene();

        if (scene.name == "TestScene")
        {
            MapWidth = 10;
            TargetAmount = 3;
            Level = 0;
        }
        else if (scene.name == "Level_1")
        {
            MapWidth = 10;
            TargetAmount = 15;
            Level = 1;
        }
        else if (scene.name == "Level_2")
        {
            MapWidth = 10;
            TargetAmount = 30;
            Level = 2;
        }
        LevelTargetAmount = TargetAmount;
    }

    void Update() 
    {
        if (TargetAmount == 0)
        {
            if (GameObject.FindGameObjectsWithTag("Target").Length == 0)
            {
                EndLevel();
            }
        }
    }

    public void HeadShot()
    {
        _HeadShot.Play();
        Score += 3;
        Headshots ++;
    }

    public void Hit()
    {
        _BodyShot.Play();
        Score ++;
    }

    public void Hostage()
    {
        _BodyShot.Play();
        Score -= 5;
    }

    void EndLevel()
    {
        ScoreHolder.SaveScores(Score, Headshots, LevelTargetAmount, Level);
        DontDestroyOnLoad(ScoreH);
        SceneManager.LoadScene("ScoreScreen");
    }
}
