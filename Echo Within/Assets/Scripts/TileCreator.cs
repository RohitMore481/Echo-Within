#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

public class TileCreator
{
    [MenuItem("Tools/Create Wall Tile")]
    public static void CreateWallTile()
    {
        CreateTile("WallTile", "Assets/WallTile.asset");
    }

    [MenuItem("Tools/Create Floor Tile")]
    public static void CreateFloorTile()
    {
        CreateTile("FloorTile", "Assets/FloorTile.asset");
    }

    private static void CreateTile(string tileName, string path)
    {
        Tile tile = ScriptableObject.CreateInstance<Tile>();
        AssetDatabase.CreateAsset(tile, path);
        AssetDatabase.SaveAssets();
        Debug.Log(tileName + " created at: " + path);
    }
}
#endif
