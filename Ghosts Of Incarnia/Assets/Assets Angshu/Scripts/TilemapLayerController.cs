using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapLayerController : MonoBehaviour
{
    public string floorLayerName = "Floor";
    public string wallLayerName = "Wall";

    void Start()
    {
        // Get the layer IDs
        int floorLayer = LayerMask.NameToLayer(floorLayerName);
        int wallLayer = LayerMask.NameToLayer(wallLayerName);

        // Ignore collision between Wall and Floor if Wall is behind Floor
        Physics2D.IgnoreLayerCollision(floorLayer, wallLayer, IsWallBehindFloor());
    }
    private void Update()
    {
        int floorLayer = LayerMask.NameToLayer(floorLayerName);
        int wallLayer = LayerMask.NameToLayer(wallLayerName);

        Physics2D.IgnoreLayerCollision(floorLayer, wallLayer, IsWallBehindFloor());
    }

    bool IsWallBehindFloor()
    {
        // Check if the Wall layer is behind the Floor layer based on their order in layer
        return Physics2D.GetLayerCollisionMask(LayerMask.NameToLayer("Floor")) > Physics2D.GetLayerCollisionMask(LayerMask.NameToLayer("Wall"));
    }
}
