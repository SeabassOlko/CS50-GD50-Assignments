using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

    public float _sensitivity = 500f;
    float _XRotation = 0f;
    float _YRotation = 0f;

    public Transform Gun;

    void Start() 
    {
        _sensitivity = GameObject.Find("_SettingsHolder").GetComponent<SettingsHolder>().sensitivity;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;    
    }

    // Update is called once per frame
    void Update()
    {
        
        float mouseX = Input.GetAxis("Mouse X") * _sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _sensitivity * Time.deltaTime;

        // clamp player x rotation to 180 degrees
        _XRotation -= mouseY;
        _XRotation = Mathf.Clamp(_XRotation, -90f, 90f);

        // clamp player y rotation to 180 degrees
        _YRotation += mouseX;
        _YRotation = Mathf.Clamp(_YRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(_XRotation, _YRotation, 0f);
    }
}
