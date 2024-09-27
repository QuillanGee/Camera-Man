using UnityEngine;

public class PickUpPlaceBlock : MonoBehaviour
{
    public Camera playerCamera; // Reference to the camera
    public LayerMask blockLayer; // Layer that defines which objects can be picked up
    public float pickupDistance = 5f; // Maximum distance to pick up objects
    public Transform holdPosition; // Where the block will be held when picked up

    private GameObject pickedBlock = null; // The currently picked-up block

    void Update()
    {
        // Check if the right mouse button is pressed
        if (Input.GetMouseButtonDown(1))
        {
            // If already holding a block, place it
            if (pickedBlock)
            {
                PlaceBlock();
            }
            else
            {
                // Otherwise, try to pick up a block
                PickUpBlock();
            }
        }

        // If we are holding a block, move it with the player or camera
    }

    // Method to pick up the block
    void PickUpBlock()
    {
        // Raycast from the center of the camera
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupDistance, blockLayer))
        {
            // If we hit a block, pick it up
            pickedBlock = hit.collider.gameObject;
            pickedBlock.GetComponent<Rigidbody>().isKinematic = true; // Disable physics on the block
            pickedBlock.GetComponent<Collider>().enabled = false;
            pickedBlock.transform.SetParent(holdPosition);
            HoldBlock();
        }
    }

    // Method to hold the block in front of the player or camera
    void HoldBlock()
    {
        // Move the block to the hold position (e.g., in front of the camera)
        pickedBlock.transform.position = holdPosition.position;
        print("Hold position: " + holdPosition.position);
        pickedBlock.transform.rotation = holdPosition.rotation;
    }

    // Method to place the block
    void PlaceBlock()
    {
        // Enable physics again for the block
        pickedBlock.GetComponent<Rigidbody>().isKinematic = false;
        pickedBlock.GetComponent<Collider>().enabled = true;
        pickedBlock.transform.parent = null;
        pickedBlock = null; // Clear the reference to the block
    }
}