using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    [SerializeField] private GameObject currProjected3DObject;
    [SerializeField] private ObjectProjection objectProjection;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private GameObject alan;
    [SerializeField] private GameObject alan2D;
    private bool isTwoD = true;

    // Update is called once per frame

    void Start()
    {
        objectProjection.UpdatePerception();
        currProjected3DObject.SetActive(false);
        alan.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            TogglePerspective();
        }
    }

    void TogglePerspective()
    {
        cameraController.UpdateCamera();
        if (!isTwoD)
        {
            isTwoD = true;
            alan.SetActive(false);
            alan2D.SetActive(true);
            objectProjection.UpdatePerception();
            currProjected3DObject.SetActive(false);
        }
        else
        {
            isTwoD = false;
            alan.SetActive(true);
            alan2D.SetActive(false);
            currProjected3DObject.SetActive(true);
            objectProjection.DestoryProjectedMesh();
        }
    }
}
