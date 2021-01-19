using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreHolder : MonoBehaviour
{
    public int Level = 0;
    public int Score = 0;
    public int HeadShots = 0;
    public int Targets = 0;

    public void SaveScores(int scr, int hdSht, int targets, int lvl)
    {
        Score = scr;
        HeadShots = hdSht;
        Targets = targets;
        Level = lvl;
    }

    public void GetScores(out int scr, out int hdSht, out int targets, out int level)
    {
        scr = Score;
        hdSht = HeadShots;
        targets = Targets;
        level = Level;
    }
}
