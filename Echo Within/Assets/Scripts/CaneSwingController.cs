using UnityEngine;

public class CaneSwingController : MonoBehaviour
{
    public Transform arm;       // Upper arm (parent)
    public Transform forearm;   // Forearm (child of arm)

    [Header("Swing Settings")]
    public float swingSpeed = 5f;

    public float forearmLeftAngle = 30f;     // Q
    public float armRightAngle = -40f;       // E

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) // Forearm swings left
        {
            StopAllCoroutines();
            StartCoroutine(SwingPart(forearm, forearmLeftAngle));
        }
        else if (Input.GetKeyDown(KeyCode.E)) // Whole arm swings right
        {
            StopAllCoroutines();
            StartCoroutine(SwingPart(arm, armRightAngle));
        }
    }

    System.Collections.IEnumerator SwingPart(Transform part, float angle)
    {
        float duration = 0.3f;
        float elapsed = 0f;

        Quaternion startRot = part.localRotation;
        Quaternion targetRot = Quaternion.Euler(0f, 0f, angle);

        // Swing
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime * swingSpeed;
            part.localRotation = Quaternion.Slerp(startRot, targetRot, elapsed / duration);
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);

        // Return to original
        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime * swingSpeed;
            part.localRotation = Quaternion.Slerp(targetRot, startRot, elapsed / duration);
            yield return null;
        }
    }
}
