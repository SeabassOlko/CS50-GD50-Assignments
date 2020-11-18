using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressEnter : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
		if (Input.GetAxis("Submit") == 1) {	
		    SceneManager.LoadScene("MainMenu");
		}
    }
}
