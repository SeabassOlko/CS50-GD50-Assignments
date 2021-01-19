using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private GameObject GameControllerObject;
    public GameObject GunObj;

    private Gun Gun;
    private GameController gameController;

    public Text Ammo;
    public Text Score;

    void Start() 
    {
        GameControllerObject = GameObject.Find("GameController");
        gameController = GameControllerObject.GetComponent<GameController>();
        Gun = GunObj.GetComponent<Gun>();
    }

    // Update is called once per frame
    void Update()
    {
        Ammo.text = "Ammo: " +Gun.CurrentAmmo;
        Score.text = "Score: " +gameController.Score;
    }
}
