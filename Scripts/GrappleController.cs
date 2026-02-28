using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(LineRenderer))]
public class GrappleController : MonoBehaviour
{
    [Header("Grapple Settings")]
    [Tooltip("How fast the player accelerates towards the target.")]
    public float pullForce = 60f;
    [Tooltip("Maximum speed to prevent breaking physics.")]
    public float maxSpeed = 25f;
    [Tooltip("Which layers can the player grapple to?")]
    public LayerMask grappleLayer;
    [Tooltip("Layers that block the grapple (e.g., walls, ground).")]
    public LayerMask obstacleLayer;
    [Tooltip("Radius around the mouse click to detect a grapple point (makes clicking easier).")]
    public float clickForgivenessRadius = 0.5f;

    private Rigidbody2D rb;
    private LineRenderer lineRenderer;

    private bool isGrappling = false;
    private Vector2 grapplePoint;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();

        // Ensure the ball doesn't spin, as requested
        rb.freezeRotation = true;

        // Hide the rope at start
        lineRenderer.enabled = false;
    }

    void Update()
    {
        HandleInput();
        UpdateVisuals();
    }

    void FixedUpdate()
    {
        // Physics updates must happen in FixedUpdate
        if (isGrappling)
        {
            rb.gravityScale = 0.2f;
            ApplyGrappleForce();
        }
        else
        {
            rb.gravityScale = 0.6f;
        }
    }

    private void HandleInput()
    {
        // Mouse Down: Try to attach
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            // 1. Check if the mouse clicked near a valid grapple surface
            Collider2D targetHit = Physics2D.OverlapCircle(mousePos, clickForgivenessRadius, grappleLayer);
            
            if (targetHit != null)
            {
                // 2. Line of Sight Check: Fire a ray from the player to the mouse position
                Vector2 direction = mousePos - (Vector2)transform.position;
                float distance = direction.magnitude;
                
                // Check if anything on the obstacleLayer is in the way
                RaycastHit2D obstruction = Physics2D.Raycast(transform.position, direction.normalized, distance, obstacleLayer);

                // If the raycast hits NOTHING, we have a clear line of sight
                if (obstruction.collider == null)
                {
                    isGrappling = true;
                    grapplePoint = mousePos; 
                }
            }
        }
        // Mouse Up: Release the grapple
        else if (Input.GetMouseButtonUp(0))
        {
            isGrappling = false;
        }
    }

    private void ApplyGrappleForce()
    {
        // Calculate the direction from the player to the grapple point
        Vector2 direction = (grapplePoint - (Vector2)transform.position).normalized;

        // Apply continuous force while held
        rb.AddForce(direction * pullForce);

        // Clamp the velocity so the player doesn't accelerate to infinity
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }

    private void UpdateVisuals()
    {
        if (isGrappling)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, grapplePoint);
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }
}