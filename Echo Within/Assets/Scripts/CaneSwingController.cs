using UnityEngine;
using System.Collections;

public class CaneSwingController : MonoBehaviour
{
    public Transform arm;
    public Transform forearm;
    public Transform caneTip;

    [Header("Swing Settings")]
    public float swingSpeed = 5f;

    [Header("Swing Angles")]
    public float forearmLeftAngle = 30f;
    public float armRightAngle = -40f;

    [Header("Forward Poke Settings")]
    public float pokeDistance = 0.5f;
    public float pokeDuration = 0.1f;

    private bool isPoking = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CaneWallDetector.lastCaneDirection = CaneWallDetector.CaneDirection.Left;
            StartSwing(forearm, forearmLeftAngle);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            CaneWallDetector.lastCaneDirection = CaneWallDetector.CaneDirection.Right;
            StartSwing(arm, armRightAngle);
        }
        else if (Input.GetKeyDown(KeyCode.F) && !isPoking)
        {
            CaneWallDetector.lastCaneDirection = CaneWallDetector.CaneDirection.Center;
            StartCoroutine(CanePokeForward());
        }
    }

    void StartSwing(Transform part, float angle)
    {
        StopAllCoroutines();
        StartCoroutine(SwingPart(part, angle));
    }

    IEnumerator SwingPart(Transform part, float angle)
    {
        float duration = 0.3f;
        float elapsed = 0f;
        Quaternion startRot = part.localRotation;
        Quaternion targetRot = Quaternion.Euler(0, 0, angle);

        // Swing Out
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime * swingSpeed;
            part.localRotation = Quaternion.Slerp(startRot, targetRot, elapsed / duration);
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);
        elapsed = 0f;

        // Return
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime * swingSpeed;
            part.localRotation = Quaternion.Slerp(targetRot, startRot, elapsed / duration);
            yield return null;
        }

        part.localRotation = startRot;
    }

    IEnumerator CanePokeForward()
    {
        isPoking = true;

        Vector3 startPos = caneTip.localPosition;
        Vector3 targetPos = startPos + new Vector3(0f, pokeDistance, 0f);

        float elapsed = 0f;
        while (elapsed < pokeDuration)
        {
            elapsed += Time.deltaTime;
            caneTip.localPosition = Vector3.Lerp(startPos, targetPos, elapsed / pokeDuration);
            yield return null;
        }

        yield return new WaitForSeconds(0.05f);

        elapsed = 0f;
        while (elapsed < pokeDuration)
        {
            elapsed += Time.deltaTime;
            caneTip.localPosition = Vector3.Lerp(targetPos, startPos, elapsed / pokeDuration);
            yield return null;
        }

        caneTip.localPosition = startPos;
        isPoking = false;
    }
}
