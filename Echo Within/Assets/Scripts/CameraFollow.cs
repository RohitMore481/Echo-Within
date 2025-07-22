using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;          // Assign your Player here in the Inspector
    public Vector3 offset = new Vector3(0, 0, -10);  // Default 2D offset

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }
}
