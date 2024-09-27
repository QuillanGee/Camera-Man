using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alan : MonoBehaviour
{
    private FirstPersonCharacterMovement fp;
    private TwoDCharacterMovement twoD;
    public Canvas crossBar;

    // Start is called before the first frame update
    void Start()
    {
        fp = GetComponent<FirstPersonCharacterMovement>();
        twoD = GetComponent<TwoDCharacterMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) // Forward movement
        {
            fp.enabled = !fp.enabled;
            twoD.enabled = !twoD.enabled;
            crossBar.enabled = !crossBar.enabled;
            
        }
    }
}
