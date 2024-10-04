using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera TwoDCam;
    public Camera FirstPersonCam;

    // Start is called before the first frame update
    void Awake()
    {
        // GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }

    void OnDestroy()
    {
        // GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    // private void GameManagerOnGameStateChanged(GameState state)
    // {
    //     if (state = GameState.FirstPerson)
    //     {
    //         TwoDCam.enabled = false;
    //     }
    // }
    
    // Update is called once per frame

    public void UpdateCamera()
    {
        TwoDCam.enabled = !TwoDCam.enabled;
        FirstPersonCam.enabled = !FirstPersonCam.enabled;
    }
}
