using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    private LineRenderer lineR;
    private Vector3 grapplePoint;
    public LayerMask grappleSurface;

    public Transform gunTip, camera, player;
    
    private static SpringJoint joint;

    public Rigidbody playerBody;

    private float maxDistance = 35f;

    private void Awake() 
    {
        lineR = GetComponent<LineRenderer>();
    }

    void Update() 
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartGrapple();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopGrapple();
        }
    }

    private void LateUpdate() 
    {
        DrawRope();
    }

    void StartGrapple()
    {
        RaycastHit hit;

        if (Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, grappleSurface))
        {
            // connect joint to grapple target
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            // The distance the grapple will keep from grapple point
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            // Spring joint values that can be changed for different effects
            joint.spring = 7f;
            joint.damper = 12f;
            joint.massScale = 6f;

            lineR.positionCount = 2;
        }
    }

    void DrawRope()
    {
        // dont draw grapple if no joint
        if (!joint) return;

        lineR.SetPosition(0, gunTip.position);
        lineR.SetPosition(1, grapplePoint);
    }
    void StopGrapple()
    {
        lineR.positionCount = 0;
        Destroy(joint);
    }

    public bool IsRope()
    {
        return joint;
    }

}