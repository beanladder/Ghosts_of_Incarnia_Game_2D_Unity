using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class WalkerGenerator : MonoBehaviour
{

    public enum Grid
    { 
        FLOOR,

        WALL,

        EMPTY
    
    }


    public Grid[,] gridHandler;
    public List<WalkerObject> Walkers;
    public Tilemap tileMap;
    public Tilemap wallTileMap;
    public List<Tile> floorTiles;  // List of floor tiles
    public Tile Wall;
    public int MapWidth = 30;
    public int MapHeight = 30;

    public Tilemap treeTileMap;
    public Tilemap bushTileMap;
    public List<Tile> bushTiles;
    public List<Tile> treeTiles;  // List of tree tiles
    public float BushPlacementProbability;
    public float TreePlacementProbability ;  // Adjust this value to control tree placement frequency
    public float TreePlacementProbabilityOnEdge; // Adjust this value as needed


    public int MaximumWalkers = 10;
    public int TileCount = default;
    public float FillPercentage = 0.4f;
    public float WaitTime = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        InitializeGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitializeGrid()
    {
        gridHandler = new Grid[MapWidth, MapHeight];

        for (int x = 0; x < gridHandler.GetLength(0); x++)
        {
            for (int y = 0; y < gridHandler.GetLength(1); y++)
            {
                gridHandler[x, y] = Grid.EMPTY;
            }
        }

        Walkers = new List<WalkerObject>();

        Vector3Int TileCenter = new Vector3Int(gridHandler.GetLength(0) / 2, gridHandler.GetLength(1) / 2, 0);

        int randomIndex = Random.Range(0, floorTiles.Count);
        Tile randomFloorTile = floorTiles[randomIndex];
        tileMap.SetTile(TileCenter, randomFloorTile);

        WalkerObject curWalker = new WalkerObject(new Vector2(TileCenter.x, TileCenter.y), GetDirection(), 0.5f);
        gridHandler[TileCenter.x, TileCenter.y] = Grid.FLOOR;
        Walkers.Add(curWalker);

        TileCount++;

        
        
        StartCoroutine(CreateFloors());
        wallTileMap.ClearAllTiles();
        
    }



    Vector2 GetDirection()
    {
        int choice = Mathf.FloorToInt(UnityEngine.Random.value * 3.99f);

        switch (choice)
        {
            case 0:
                return Vector2.down;
            case 1: return Vector2.left;
                case 2: return Vector2.up;
                case 3: return Vector2.right;
                default : return Vector2.zero;
        }
    }


    //Eta likhte giye Coffee pore gechilo pant e
    IEnumerator CreateFloors()
    {
        while ((float)TileCount / (float)gridHandler.Length < FillPercentage)
        {
            bool hasCreatedFloor = false;
            foreach (WalkerObject curWalker in Walkers)
            {
                Vector3Int curPos = new Vector3Int((int)curWalker.Position.x, (int)curWalker.Position.y, 0);

                if (gridHandler[curPos.x, curPos.y] != Grid.FLOOR)
                {
                    // Choose a random floor tile from the list
                    Tile randomFloorTile = floorTiles[Random.Range(0, floorTiles.Count)];
                    tileMap.SetTile(curPos, randomFloorTile);

                    TileCount++;
                    gridHandler[curPos.x, curPos.y] = Grid.FLOOR;
                    hasCreatedFloor = true;
                }
            }

            //Walker Functions, Ignore if Straight
            ChanceToRemove();
            ChanceToRedirect();
            ChanceToCreate();
            UpdatePosition();

            if (hasCreatedFloor)
            {
                yield return new WaitForSeconds(WaitTime);
            }
        }

        // Place trees after creating floors
        PlaceTrees();
    }

    void PlaceTrees()
    {
        foreach (WalkerObject curWalker in Walkers)
        {
            Vector3Int curPos = new Vector3Int((int)curWalker.Position.x, (int)curWalker.Position.y, 0);

            if (gridHandler[curPos.x, curPos.y] == Grid.FLOOR && Random.value < TreePlacementProbability)
            {
                PlaceTreeOnFloor(curPos);
            }
        }

        PlaceBush();
        StartCoroutine(CreateWallsAndPlaceTrees());
    }

    void PlaceBush()
    {
        foreach (WalkerObject curWalker in Walkers)
        {
            Vector3Int curPos = new Vector3Int((int)curWalker.Position.x, (int)curWalker.Position.y, 0);

            if (gridHandler[curPos.x, curPos.y] == Grid.FLOOR && Random.value < BushPlacementProbability)
            { 
                 PlaceBushOnFloor(curPos);
            }
        }
    }

    void PlaceTreeOnFloor(Vector3Int position)
    {
        Tile randomTreeTile = treeTiles[Random.Range(0, treeTiles.Count)];

        if (treeTileMap.GetTile(position) == null)
        {
            treeTileMap.SetTile(position, randomTreeTile);
        }
    }

    void PlaceBushOnFloor(Vector3Int position)
    {
        Tile randombushTile = bushTiles[Random.Range(0, bushTiles.Count)];

        if (bushTileMap.GetTile(position) == null)
        {
            bushTileMap.SetTile(position, randombushTile);
        }
    }
    void PlaceTreesNearTile(Vector3Int position)
    {
        int treePlacementDistance = 2;

        for (int x = position.x - treePlacementDistance; x <= position.x + treePlacementDistance; x++)
        {
            for (int y = position.y - treePlacementDistance; y <= position.y + treePlacementDistance; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);

                if (IsWithinBounds(pos) && gridHandler[pos.x, pos.y] == Grid.EMPTY)
                {
                    float distanceFactor = Mathf.Clamp01((pos - position).magnitude / treePlacementDistance);
                    float placementProbability = IsEdge(pos) ? TreePlacementProbabilityOnEdge : TreePlacementProbability;

                    if (Random.value < placementProbability * distanceFactor)
                    {
                        Tile randomTreeTile = treeTiles[Random.Range(0, treeTiles.Count)];

                        if (treeTileMap.GetTile(pos) == null)
                        {
                            treeTileMap.SetTile(pos, randomTreeTile);
                        }
                    }
                }
            }
        }
    }

    bool IsEdge(Vector3Int pos)
    {
        return pos.x == 0 || pos.x == MapWidth - 1 || pos.y == 0 || pos.y == MapHeight - 1;
    }

    bool IsWithinBounds(Vector3Int pos)
    {
        return pos.x >= 0 && pos.x < MapWidth && pos.y >= 0 && pos.y < MapHeight;
    }


    //Suswata eta dhorle tor baba amar
    void ChanceToRemove()
    { 
        int updatedCount = Walkers.Count;
        for (int i = 0; i < updatedCount; i++)
        {
            if (UnityEngine.Random.value < Walkers[i].ChanceToChange && Walkers.Count > 1)
            {
                Walkers.RemoveAt(i);
                break;
            }
        }
    }

    //Suswata eta Dhorle tor maa amar
    void ChanceToRedirect()
    {
        for (int i = 0; i < Walkers.Count; i++)
        {
            if (UnityEngine.Random.value < Walkers[i].ChanceToChange)
            { 
                WalkerObject curWalker = Walkers[i];
                curWalker.Direction = GetDirection();
                Walkers[i] = curWalker;
            }
        }
    }

    //Suswata eta dhorle tor maa amar
    void ChanceToCreate()
    {
        int updatedCount = Walkers.Count;
        for (int i = 0; i < updatedCount; i++)
        {
            if (UnityEngine.Random.value < Walkers[i].ChanceToChange && Walkers.Count < MaximumWalkers)
            {
                Vector2 newDirection = GetDirection();
                Vector2 newPosition = Walkers[i].Position;

                WalkerObject newWalker = new WalkerObject(newPosition, newDirection, 0.5f);
                Walkers.Add(newWalker);
            }
        }
    }

    //Suswata eta dhorle Chandrima gei
    void UpdatePosition()
    {
        for (int i = 0; i < Walkers.Count; i++)
        { 
            WalkerObject FoundWalker = Walkers[i];
            FoundWalker.Position += FoundWalker.Direction;
            FoundWalker.Position.x = Mathf.Clamp(FoundWalker.Position.x, 1, gridHandler.GetLength(0) - 2);
            FoundWalker.Position.y = Mathf.Clamp(FoundWalker.Position.y, 1, gridHandler.GetLength(1) - 2);
            Walkers[i] = FoundWalker;
        }
    }

    //Eta dhora jabe kintu Walls jodi na place hoy tahole kortipokho dai noy ;)
    IEnumerator CreateWallsAndPlaceTrees()
    {
        for (int x = 0; x < gridHandler.GetLength(0) - 1; x++)
        {
            for (int y = 0; y < gridHandler.GetLength(1) - 1; y++)
            {
                if (gridHandler[x, y] == Grid.FLOOR)
                {
                    bool hasCreatedWall = false;

                    if (gridHandler[x + 1, y] == Grid.EMPTY)
                    {
                        wallTileMap.SetTile(new Vector3Int(x + 1, y, 0), Wall);
                        gridHandler[x + 1, y] = Grid.WALL;
                        hasCreatedWall = true;
                    }

                    if (gridHandler[x - 1, y] == Grid.EMPTY)
                    {
                        wallTileMap.SetTile(new Vector3Int(x - 1, y, 0), Wall);
                        gridHandler[x - 1, y] = Grid.WALL;
                        hasCreatedWall = true;
                    }

                    if (gridHandler[x, y + 1] == Grid.EMPTY)
                    {
                        wallTileMap.SetTile(new Vector3Int(x, y + 1, 0), Wall);
                        gridHandler[x, y + 1] = Grid.WALL;
                        hasCreatedWall = true;
                    }

                    if (gridHandler[x, y - 1] == Grid.EMPTY)
                    {
                        wallTileMap.SetTile(new Vector3Int(x, y - 1, 0), Wall);
                        gridHandler[x, y - 1] = Grid.WALL;
                        hasCreatedWall = true;
                    }

                    if (hasCreatedWall)
                    {
                        yield return new WaitForSeconds(WaitTime);
                    }
                }
            }
        }

        // After walls are generated, place trees on top of wall tiles
        //PlaceTreesOnWalls();
    }

    void PlaceTreesOnWalls()
    {
        for (int x = 0; x < gridHandler.GetLength(0); x++)
        {
            for (int y = 0; y < gridHandler.GetLength(1); y++)
            {
                if (gridHandler[x, y] == Grid.WALL)
                {
                    // Place trees with probability on top of wall tiles
                    if (Random.value < TreePlacementProbability)
                    {
                        Tile randomTreeTile = treeTiles[Random.Range(0, treeTiles.Count)];

                        // Set the tree tile only if there is no existing tile at the position
                        if (treeTileMap.GetTile(new Vector3Int(x, y, 0)) == null)
                        {
                            treeTileMap.SetTile(new Vector3Int(x, y, 0), randomTreeTile);
                        }
                    }
                }
            }
        }
    }
}
