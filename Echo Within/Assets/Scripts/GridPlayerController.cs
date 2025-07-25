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

        PrintGridPosition(); // Print starting grid position
    }

    void Update()
    {
        if (!isMoving)
        {
            Vector2 input = GetInput();
            if (input != Vector2.zero)
            {
                Vector2 direction = input.normalized;

                // Rotate based on direction
                if (direction == Vector2.down)        baseRotation = Quaternion.Euler(0f, 0f, 0f);
                else if (direction == Vector2.right)  baseRotation = Quaternion.Euler(0f, 0f, 90f);
                else if (direction == Vector2.up)     baseRotation = Quaternion.Euler(0f, 0f, 180f);
                else if (direction == Vector2.left)   baseRotation = Quaternion.Euler(0f, 0f, 270f);

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

        return hitCount == 0;
    }

    IEnumerator MoveToPosition(Vector2 destination, Vector2 direction)
    {
        isMoving = true;

        Vector2 start = rb.position;
        float elapsed = 0f;
        float duration = 1f / moveSpeed;

        // Tilt during movement
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
        transform.rotation = baseRotation;
        isMoving = false;

        PrintGridPosition();
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

    void PrintGridPosition()
    {
        int x = Mathf.RoundToInt(transform.position.x);
        int y = Mathf.RoundToInt(transform.position.y);
        Debug.Log($"Player Grid Position: ({x}, {y})");
    }
}
