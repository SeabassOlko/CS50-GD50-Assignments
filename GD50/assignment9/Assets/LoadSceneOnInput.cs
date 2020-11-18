using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnInput : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Scene scene = SceneManager.GetActiveScene();
		if (Input.GetAxis("Submit") == 1) {
			if (scene.name == "Title") {
				SceneManager.LoadScene("Play");
			}else {
				SceneManager.LoadScene("Title");
			}
		}
	}
}
