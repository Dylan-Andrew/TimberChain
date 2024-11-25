using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject player;              // Reference to the player object
    [SerializeField]
    private GameObject grappleHook;          // Reference to the grapple hook object
    [SerializeField]
    private LineRenderer chainRenderer;     // LineRenderer for the chain
    [SerializeField]
    private LayerMask branchLayer;          // Layer mask for detecting branches
    [SerializeField]
    private float grappleSpeed = 10f;       // Speed at which the grapple hook moves
    [SerializeField]
    private float pullSpeed = 5f;           // Speed at which the chain retracts

    private Vector3 grappleTarget;          // Target position for the grapple hook
    private bool isExtending = false;       // Flag to indicate if the hook is extending
    private bool isRetracting = false;      // Flag to indicate if the player is being pulled
    private GameObject currentBranch;       // The branch we're grappling to
    private Rigidbody2D playerRigidbody;    // Reference to the player's Rigidbody2D
    private DistanceJoint2D distanceJoint;  // DistanceJoint2D for simulating chain shortening

    void Start()
    {
        // Initialize the player's Rigidbody2D and DistanceJoint2D
        playerRigidbody = player.GetComponent<Rigidbody2D>();
        distanceJoint = player.GetComponent<DistanceJoint2D>();

        // Disable the joint at start until we begin grappling
        distanceJoint.enabled = false;

        // Set up the LineRenderer to have 2 points: start (player) and end (hook)
        chainRenderer.positionCount = 2;

        // Initially hide the grapple hook and chain
        grappleHook.SetActive(false);
        chainRenderer.enabled = false;
    }

    void Update()
    {
        // Detect mouse click for grappling
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0;

            // Perform a raycast to detect if we hit a branch
            RaycastHit2D hit = Physics2D.Raycast(player.transform.position, mouseWorldPosition - player.transform.position, Mathf.Infinity, branchLayer);

            if (hit.collider != null)
            {
                // Set target and begin grappling
                grappleTarget = hit.point;
                currentBranch = hit.collider.gameObject;
                isExtending = true;
                isRetracting = false;

                // Move the hook to the player's position initially
                grappleHook.transform.position = player.transform.position;

                // Show the grapple hook and chain when it's extending
                grappleHook.SetActive(true);
                chainRenderer.enabled = true;
            }
        }

        // Handle extending and retracting of the grapple
        if (isExtending)
        {
            ExtendGrapple();
        }

        if (isRetracting)
        {
            RetractChain();
        }

        // Handle branch breakage
        if (currentBranch == null && distanceJoint.enabled)
        {
            // If the branch breaks, disable the joint and allow the hook to follow the player
            Debug.Log("Branch broke!");

            // Disable the DistanceJoint2D to stop pulling the player
            distanceJoint.enabled = false;
            isRetracting = false;

            // Hide the grapple hook and chain when the branch breaks
            grappleHook.SetActive(false);
            chainRenderer.enabled = false;
        }

        // Update the chain to connect player and hook
        UpdateChain();
    }

    void ExtendGrapple()
    {
        // Move the hook towards the grapple target
        float step = grappleSpeed * Time.deltaTime;
        grappleHook.transform.position = Vector3.MoveTowards(grappleHook.transform.position, grappleTarget, step);

        // Stop extending when hook reaches target
        if (Vector3.Distance(grappleHook.transform.position, grappleTarget) < 0.1f)
        {
            isExtending = false;

            // When hook reaches the target, enable the DistanceJoint and start retracting
            distanceJoint.connectedAnchor = grappleHook.transform.position;
            distanceJoint.distance = Vector2.Distance(player.transform.position, grappleHook.transform.position);
            distanceJoint.enabled = true;
            isRetracting = true;
        }
    }

    void RetractChain()
    {
        float minDistance = 0.5f; // Minimum distance to simulate hanging
        distanceJoint.distance = Mathf.MoveTowards(distanceJoint.distance, minDistance, pullSpeed * Time.deltaTime);

        // Stop retracting when player reaches minimum distance
        if (distanceJoint.distance <= minDistance)
        {
            distanceJoint.distance = minDistance;
            isRetracting = false;

            // Hide the grapple hook and chain once the retracting is done
            grappleHook.SetActive(false);
            chainRenderer.enabled = false;
        }
    }

    void UpdateChain()
    {
        chainRenderer.SetPosition(0, player.transform.position);
        chainRenderer.SetPosition(1, grappleHook.transform.position);
    }
}
