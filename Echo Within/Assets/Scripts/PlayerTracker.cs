using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerGridTracker : MonoBehaviour
{
    public Tilemap tilemap;  // Assign in Inspector
    public Transform player; // Drag the player Transform here

    void Update()
    {
        if (tilemap != null && player != null)
        {
            Vector3Int gridPos = tilemap.WorldToCell(player.position);
            
        }
    }
}
