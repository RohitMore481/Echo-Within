using UnityEngine;
using UnityEngine.Tilemaps;

public class CaneWallDetector : MonoBehaviour
{
    public enum CaneDirection
    {
        Left,
        Right,
        Center
    }

    public static CaneDirection lastCaneDirection = CaneDirection.Center;

    private Tilemap wallTilemap;

    void Start()
    {
        wallTilemap = GameObject.Find("Wall Tilemap").GetComponent<Tilemap>();

        if (wallTilemap == null)
        {
            Debug.LogError("Wall Tilemap not found! Make sure it's named exactly 'Wall Tilemap'");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            Vector3 worldPos = transform.position;
            Vector3Int gridPos = wallTilemap.WorldToCell(worldPos);

            string directionText = lastCaneDirection.ToString();
            Debug.Log($"Cane touched tile at Grid Position: {gridPos}, Direction: {directionText}");

            // Optional: fade the tile on touch
            TileFader tileFader = collision.GetComponent<TileFader>();
            if (tileFader != null)
            {
                tileFader.FadeSelf(); // Make sure FadeSelf exists
            }
        }
    }
}
