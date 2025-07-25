using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FadeOnTouch : MonoBehaviour
{
    private SpriteRenderer sr;
    public float fadeDuration = 1f;
    private bool isFading = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        // Make tile fully invisible at start
        Color color = sr.color;
        color.a = 0f;
        sr.color = color;
    }

    public void FadeIn()
    {
        if (!isFading)
        {
            StartCoroutine(FadeToVisible());
        }
    }

    System.Collections.IEnumerator FadeToVisible()
    {
        isFading = true;

        float elapsed = 0f;
        Color startColor = sr.color;
        Color targetColor = startColor;
        targetColor.a = 1f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            sr.color = Color.Lerp(startColor, targetColor, elapsed / fadeDuration);
            yield return null;
        }

        sr.color = targetColor;
        isFading = false;
    }

    // Optional: method that matches CaneWallDetector call
    public void FadeTileInDirection(Vector2 direction)
    {
        FadeIn();
        Debug.Log("FadeTileInDirection called with direction: " + direction);
    }
}
