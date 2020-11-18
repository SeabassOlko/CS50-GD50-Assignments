using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelText : MonoBehaviour
{
    //public LevelGenerator levelGenerator;
	private Text text;
	private int level;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        level = LevelGenerator.level;
        text.text = "Level: " + level;
    }
}
