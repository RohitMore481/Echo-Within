using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

[RequireComponent(typeof(Tilemap), typeof(TilemapRenderer))]
public class TileFader : MonoBehaviour
{
    private Tilemap tilemap;
    private TilemapRenderer tilemapRenderer;

    [Header("Fade Settings")]
    public float fadeDuration = 0.5f;
    public float visibleDuration = 1.5f; // How long it stays visible before fading out

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        tilemapRenderer = GetComponent<TilemapRenderer>();
    }

    private void Start()
    {
        // Immediately make tilemap invisible at game start
        SetAlpha(0f);
    }

    public void FadeSelf()
    {
        StopAllCoroutines();
        StartCoroutine(FadeInOutRoutine());
    }

    private IEnumerator FadeInOutRoutine()
    {
        // Fade In
        yield return StartCoroutine(Fade(0f, 1f, fadeDuration));

        // Stay visible
        yield return new WaitForSeconds(visibleDuration);

        // Fade Out
        yield return StartCoroutine(Fade(1f, 0f, fadeDuration));
    }

    private IEnumerator Fade(float fromAlpha, float toAlpha, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float currentAlpha = Mathf.Lerp(fromAlpha, toAlpha, t);
            SetAlpha(currentAlpha);
            yield return null;
        }

        SetAlpha(toAlpha);
    }

    private void SetAlpha(float alpha)
    {
        if (tilemapRenderer != null)
        {
            Color c = tilemapRenderer.material.color;
            tilemapRenderer.material.color = new Color(c.r, c.g, c.b, alpha);
        }
    }
}
