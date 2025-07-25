using UnityEngine;

public class CaneWallDetector : MonoBehaviour
{
    public enum CaneDirection
    {
        Left, Right, Up, Down, Center
    }

    public static CaneDirection lastCaneDirection = CaneDirection.Down;

    public FadeOnTouch fadeOnTouch;

    private void Start()
    {
        if (fadeOnTouch == null)
            fadeOnTouch = FindObjectOfType<FadeOnTouch>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) lastCaneDirection = CaneDirection.Up;
        if (Input.GetKeyDown(KeyCode.S)) lastCaneDirection = CaneDirection.Down;
        if (Input.GetKeyDown(KeyCode.A)) lastCaneDirection = CaneDirection.Left;
        if (Input.GetKeyDown(KeyCode.D)) lastCaneDirection = CaneDirection.Right;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            Vector2 direction = DirectionToVector(lastCaneDirection);
            fadeOnTouch?.FadeTileInDirection(transform.position, direction);
        }
    }

    private Vector2 DirectionToVector(CaneDirection dir)
    {
        switch (dir)
        {
            case CaneDirection.Up: return Vector2.up;
            case CaneDirection.Down: return Vector2.down;
            case CaneDirection.Left: return Vector2.left;
            case CaneDirection.Right: return Vector2.right;
            default: return Vector2.zero;
        }
    }
}
