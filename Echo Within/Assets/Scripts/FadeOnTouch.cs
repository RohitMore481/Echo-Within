using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FadeOnTouch : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public float fadeDuration = 1f;

    private bool isFading = false;
    private float fadeTimer = 0f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetAlpha(0f); // Start invisible
    }

    void Update()
    {
        if (isFading)
        {
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Clamp01(fadeTimer / fadeDuration);
            SetAlpha(alpha);

            if (alpha >= 1f)
                isFading = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Cane")) // Tag the cane GameObject with "Cane"
        {
            StartFade();
        }
    }

    public void StartFade()
    {
        if (!isFading)
        {
            isFading = true;
            fadeTimer = 0f;
        }
    }

    private void SetAlpha(float alpha)
    {
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }
    }
}
