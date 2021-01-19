using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsHolder : MonoBehaviour
{   
    public float sensitivity;

    void Start() 
    {
        sensitivity = 100;
        DontDestroyOnLoad(gameObject);
    }

    public void SetSensitivity(float sen)
    {
        sensitivity = sen;
    }

}
