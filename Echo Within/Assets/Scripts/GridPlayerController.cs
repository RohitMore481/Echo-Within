using UnityEngine;
using System.Collections;

public class GridPlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public LayerMask obstacleLayer;
    public float tiltAngle = 25f;

    public enum RotationAxis { X, Y, Z }

    public RotationAxis upAxis = RotationAxis.X;
    public RotationAxis downAxis = RotationAxis.X;
    public RotationAxis leftAxis = RotationAxis.Y;
    public RotationAxis rightAxis = RotationAxis.Y;

    private bool isMoving = false;
    private Rigidbody2D rb;
    private Collider2D col;

    private Quaternion baseRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        baseRotation = transform.rotation;
    }

    void Update()
    {
        if (!isMoving)
        {
            Vector2 input = GetInput();
            if (input != Vector2.zero)
            {
                Vector2 direction = input.normalized;

                // Rotate IMMEDIATELY based on input direction
                if (direction == Vector2.down)        // S
                    baseRotation = Quaternion.Euler(0f, 0f, 0f);
                else if (direction == Vector2.right)  // D
                    baseRotation = Quaternion.Euler(0f, 0f, 90f);
                else if (direction == Vector2.up)     // W
                    baseRotation = Quaternion.Euler(0f, 0f, 180f);
                else if (direction == Vector2.left)   // A
                    baseRotation = Quaternion.Euler(0f, 0f, 270f);

                // Apply immediately to reflect facing even if blocked
                transform.rotation = baseRotation;

                Vector2 destination = rb.position + direction;

                if (CanMoveInDirection(direction))
                {
                    StartCoroutine(MoveToPosition(destination, direction));
                }
            }
        }
    }

    Vector2 GetInput()
    {
        if (Input.GetKeyDown(KeyCode.W)) return Vector2.up;
        if (Input.GetKeyDown(KeyCode.S)) return Vector2.down;
        if (Input.GetKeyDown(KeyCode.A)) return Vector2.left;
        if (Input.GetKeyDown(KeyCode.D)) return Vector2.right;
        return Vector2.zero;
    }

    bool CanMoveInDirection(Vector2 direction)
    {
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = false;
        filter.SetLayerMask(obstacleLayer);
        filter.useLayerMask = true;

        RaycastHit2D[] results = new RaycastHit2D[1];
        int hitCount = col.Cast(direction, filter, results, 1f);

        if (hitCount > 0)
        {
            Debug.Log("Blocked by: " + results[0].collider.name);
        }

        return hitCount == 0;
    }

    IEnumerator MoveToPosition(Vector2 destination, Vector2 direction)
    {
        isMoving = true;

        Vector2 start = rb.position;
        float elapsed = 0f;
        float duration = 1f / moveSpeed;

        // Tilt the player in chosen axis while moving
        Quaternion tiltRotation = GetTiltRotation(direction);
        transform.rotation = tiltRotation * baseRotation;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            rb.MovePosition(Vector2.Lerp(start, destination, t));
            yield return null;
        }

        rb.MovePosition(destination);
        transform.rotation = baseRotation; // Stay facing final direction
        isMoving = false;
    }

    Quaternion GetTiltRotation(Vector2 direction)
    {
        RotationAxis axisToUse = RotationAxis.X;
        float angle = tiltAngle;

        if (direction == Vector2.up) axisToUse = upAxis;
        else if (direction == Vector2.down) axisToUse = downAxis;
        else if (direction == Vector2.left) axisToUse = leftAxis;
        else if (direction == Vector2.right) axisToUse = rightAxis;

        if (direction == Vector2.down || direction == Vector2.right)
            angle = -angle;

        switch (axisToUse)
        {
            case RotationAxis.X: return Quaternion.Euler(angle, 0f, 0f);
            case RotationAxis.Y: return Quaternion.Euler(0f, angle, 0f);
            case RotationAxis.Z: return Quaternion.Euler(0f, 0f, angle);
            default: return Quaternion.identity;
        }
    }
}
