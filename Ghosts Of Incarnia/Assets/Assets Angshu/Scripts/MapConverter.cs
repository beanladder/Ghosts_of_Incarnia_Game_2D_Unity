using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class MapConverter : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject floorPrefab;
    public GameObject wallPrefab;
    public GameObject treePrefab;

    void Start()
    {
        ConvertTilemapToWorld();
    }

    void ConvertTilemapToWorld()
    {
        BoundsInt bounds = tilemap.cellBounds;

        for (int x = bounds.x; x < bounds.x + bounds.size.x; x++)
        {
            for (int y = bounds.y; y < bounds.y + bounds.size.y; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                TileBase tile = tilemap.GetTile(tilePosition);

                if (tile != null)
                {
                    GameObject prefabToInstantiate = GetPrefabForTile(tile);
                    Instantiate(prefabToInstantiate, tilemap.GetCellCenterWorld(tilePosition), Quaternion.identity);
                }
            }
        }
    }

    GameObject GetPrefabForTile(TileBase tile)
    {
        // Add logic to determine which prefab corresponds to the tile
        // For example, check the tile name or properties
        if (tile.name == "FloorTile")
            return floorPrefab;
        else if (tile.name == "WallTile")
            return wallPrefab;
        else if (tile.name == "TreeTile")
            return treePrefab;
        else
            return null; // Default or handle other tiles
    }
}
