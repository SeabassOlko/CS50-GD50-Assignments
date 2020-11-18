using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchHeadCheck : MonoBehaviour
{
    public float checkDist = 0.8f;
    public LayerMask groundMask;
    public LayerMask grappleMask;
    public bool IsHeadClearance()
    {
        // create a sphere to check fi players head is clear of colliders and can stand
        if (Physics.CheckSphere(transform.position, checkDist, groundMask) || Physics.CheckSphere(transform.position, checkDist, grappleMask))
        {
            return true;
        }
        return false;
    }
}
