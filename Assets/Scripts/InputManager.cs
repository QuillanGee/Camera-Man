using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    [SerializeField] private GameObject currProjected3DObject;
    [SerializeField] private ObjectProjection objectProjection;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private GameObject alan;
    [SerializeField] private GameObject alan2D;
    [SerializeField] private PickUpPlaceBlock pickUpPlaceBlock;
    private bool isTwoD = true;

    private FirstPersonCharacterMovement firstpersonCharacterMovement;
    private TwoDCharacterMovement twoDCharacterMovement;
    private Alan2D alan2DScript;
    private Alan alanScript;
    private Canvas crossHair;

    void Start()
    {
        firstpersonCharacterMovement = alan.GetComponent<FirstPersonCharacterMovement>();
        twoDCharacterMovement = alan2D.GetComponent<TwoDCharacterMovement>();
        alan2DScript = alan2D.GetComponent<Alan2D>();
        alanScript = alan.GetComponent<Alan>();    
        crossHair = alan.GetComponentInChildren<Canvas>();
        
        alan.GetComponent<Alan>().ProjectAlan();
        if (currProjected3DObject != null)
        {
            objectProjection.UpdatePerception();
            currProjected3DObject.SetActive(false);
        }
        alan.GetComponent<FirstPersonCharacterMovement>().enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            TogglePerspective();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            #if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;  // Stops play mode in editor
            #else
                        Application.Quit();  // Quits the built application
            #endif
        }
    }

    void TogglePerspective()
    {
        cameraController.UpdateCamera();
        
        //Going to 2D
        if (!isTwoD)
        {
            isTwoD = true;
            
            //Scales and moves Alan based on where he was in 3D
            alanScript.ProjectAlan();
            
            //Toggle Player controllers
            firstpersonCharacterMovement.enabled = false;
            twoDCharacterMovement.enabled = true;
            crossHair.enabled = false;

            //PROJECT OBJECT
            if (currProjected3DObject != null)
            {
                objectProjection.UpdatePerception();
                //check if holding, if yes, attach to 2D character
                if (pickUpPlaceBlock.isHolding)
                {
                    // Determine the direction the character is facing
                    float direction = Mathf.Sign(alan2D.transform.localScale.x);

                    // Calculate the new position
                    Vector3 newPosition = alan2D.transform.position + new Vector3(direction * 0.7f, 1f, 0f);
                    
                    objectProjection.PositionBlockToHoldPosition(newPosition);
                    objectProjection.projectedMeshObject.transform.SetParent(alan2D.transform);
                }
                currProjected3DObject.SetActive(false);
            }
        }
        
        //Going to 3D
        else
        {
            isTwoD = false;
            alan2DScript.projectAlan2D();
            firstpersonCharacterMovement.enabled = true;
            twoDCharacterMovement.enabled = false;
            crossHair.enabled = true;
            
            if (currProjected3DObject != null)
            {
                currProjected3DObject.SetActive(true);
                objectProjection.DestoryProjectedMesh();
            }
        }
    }
}
