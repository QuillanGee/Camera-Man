using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alan : MonoBehaviour
{
    [SerializeField] Transform projectedWallTransform;
    [SerializeField] GameObject Alan2D;
    private Vector3 alanDefaultScale = new Vector3(3.04f,3.04f,3.04f);


    public void ProjectAlan()
    {
        //gets 2D Alan's direction
        int direction = Alan2D.transform.localScale.x > 0 ? 1 : -1;
        
        //Scales based on distance from InvisaWall
        float distanceToPlane = projectedWallTransform.position.z - transform.position.z;
        float scaleFactor =  2*(1.0f / Mathf.Max(1e-5f, Mathf.Abs(distanceToPlane))); // Avoid division by zero
        Vector3 theScale = alanDefaultScale * scaleFactor;
        theScale.x *= direction;
        Alan2D.transform.localScale = theScale;
        
        //Moves to corresponding X position
        Vector3 newXPosition = Alan2D.transform.position;
        newXPosition.x = transform.position.x;
        Alan2D.transform.position = newXPosition;
        
        //Moves to corresponding Y position
        Vector3 newYPosition = Alan2D.transform.position;
        newYPosition.y = transform.position.y;
        Alan2D.transform.position = newYPosition;
    }
    
}
