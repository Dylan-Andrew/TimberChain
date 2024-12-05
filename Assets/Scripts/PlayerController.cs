using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject grappleHook;
    [SerializeField]
    private LineRenderer chainRenderer;
    [SerializeField]
    private LayerMask branchLayer;
    [SerializeField]
    private float grappleSpeed = 10f;
    [SerializeField]
    private float pullSpeed = 5f;

    private Vector3 grappleTarget;
    private bool isExtending = false;
    private bool isRetracting = false;
    private GameObject currentBranch;
    private Rigidbody2D playerRigidbody;
    private DistanceJoint2D distanceJoint;

    void Start()
    {
        playerRigidbody = player.GetComponent<Rigidbody2D>();
        distanceJoint = player.GetComponent<DistanceJoint2D>();
        distanceJoint.enabled = false;
        chainRenderer.positionCount = 2;
        grappleHook.SetActive(false);
        chainRenderer.enabled = false;
    }

    void Update()
    {
        if (!gameManager.IsPlaying)
        {
            grappleHook.SetActive(false);
            chainRenderer.enabled = false;
            playerRigidbody.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0;
            RaycastHit2D hit = Physics2D.Raycast(player.transform.position, mouseWorldPosition - player.transform.position, Mathf.Infinity, branchLayer);

            if (hit.collider != null)
            {
                grappleTarget = hit.point;
                currentBranch = hit.collider.gameObject;
                isExtending = true;
                isRetracting = false;
                grappleHook.transform.position = player.transform.position;
                grappleHook.SetActive(true);
                chainRenderer.enabled = true;
            }
        }

        if (isExtending)
        {
            ExtendGrapple();
        }

        if (isRetracting)
        {
            RetractChain();
        }

        if (currentBranch == null && distanceJoint.enabled)
        {
            distanceJoint.enabled = false;
            isRetracting = false;
            grappleHook.SetActive(false);
            chainRenderer.enabled = false;
        }

        UpdateChain();
    }

    void ExtendGrapple()
    {
        float step = grappleSpeed * Time.deltaTime;
        grappleHook.transform.position = Vector3.MoveTowards(grappleHook.transform.position, grappleTarget, step);

        if (Vector3.Distance(grappleHook.transform.position, grappleTarget) < 0.1f)
        {
            isExtending = false;
            distanceJoint.connectedAnchor = grappleHook.transform.position;
            distanceJoint.distance = Vector2.Distance(player.transform.position, grappleHook.transform.position);
            distanceJoint.enabled = true;
            isRetracting = true;
        }
    }

    void RetractChain()
    {
        float minDistance = 0.5f;
        distanceJoint.distance = Mathf.MoveTowards(distanceJoint.distance, minDistance, pullSpeed * Time.deltaTime);

        if (distanceJoint.distance <= minDistance)
        {
            distanceJoint.distance = minDistance;
            isRetracting = false;
            grappleHook.SetActive(false);
            chainRenderer.enabled = false;
        }
    }

    void UpdateChain()
    {
        chainRenderer.SetPosition(0, player.transform.position);
        chainRenderer.SetPosition(1, grappleHook.transform.position);
    }

    public void UpdatePlayerSpeed()
    {
        grappleSpeed += 1f;
        pullSpeed += 1f;
    }
}
