 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{   
    public float checkDist = 0.4f;
    public LayerMask groundMask;
    public LayerMask grappleMask;
    public bool IsGrounded()
    {
        // create a sphere and check if the player is on the ground
        if (Physics.CheckSphere(transform.position, checkDist, groundMask) || Physics.CheckSphere(transform.position, checkDist, grappleMask))
        {
            return true;
        }
        return false;
    }
}
