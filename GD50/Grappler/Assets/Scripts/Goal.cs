using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    void OnTriggerEnter(Collider other) {
        Scene scene = SceneManager.GetActiveScene();

        if (scene.name == "Tutorial")
        {
            SceneManager.LoadScene("MainMenu");
        }
        else if (scene.name == "Level_1")
        {
            SceneManager.LoadScene("Level_2");
        }
        else
        {
            SceneManager.LoadScene("WinScreen");
        }
          
	}
}
