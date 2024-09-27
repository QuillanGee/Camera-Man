using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera TwoDCam;
    public Camera FirstPersonCam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) // Forward movement
        {
            TwoDCam.enabled = !TwoDCam.enabled;
            FirstPersonCam.enabled = !FirstPersonCam.enabled;
        }
    }
}
