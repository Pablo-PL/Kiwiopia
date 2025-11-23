using UnityEngine;
using System.Collections.Generic;



public class TilesHandler : MonoBehaviour
{
    public int gridSize;
    public GameObject hexTile;
    public List<Tile> tiles = new List<Tile>();

    void Start()
    {
        if (gridSize % 2 == 0)
        {
            gridSize += 1;
        }
        Vector2 startingPosition = new Vector2((-Mathf.Sqrt(3)/2) * (gridSize - 1), (-gridSize + 1) * 1.5f);
        float deltaX = 0f;
        for (int i = 0; i < gridSize; i++)
        {
            for (int x = 0; x < gridSize + i; x++)
            {
                Vector2 pos = startingPosition + new Vector2(x * Mathf.Sqrt(3) + deltaX, 1.5f * i);
                GameObject tileGameObject = Instantiate(hexTile, new Vector3(pos.x, 0, pos.y), Quaternion.identity);
                tileGameObject.name = $"Tile_{i}_{x}";
                tileGameObject.transform.parent = transform;
                Tile newTile = tileGameObject.AddComponent<Tile>();
                newTile.position = pos;
                newTile.type = "grass";
                
                if (x != 0) {
                    AddNeighbourAtIndex(tiles.Count - 1, newTile);
                }

                if (i != 0)
                {
                    if (x != 0)
                    {
                        AddNeighbourAtIndex(tiles.Count - 1 - x - (gridSize + i - 1) + x, newTile);
                    }
                     if (x != gridSize + i - 1)
                    {
                        AddNeighbourAtIndex(tiles.Count - 1 - x - (gridSize + i - 1) + x + 1, newTile);
                    }
                }

                tiles.Add(newTile);
            }
            deltaX -= Mathf.Sqrt(3) / 2;
        }
        deltaX += Mathf.Sqrt(3);
        for (int i = gridSize - 2; i >= 0; i--)
        {
            for (int x = 0; x < gridSize + i; x++)
            {
                Vector2 pos = startingPosition + new Vector2(x * Mathf.Sqrt(3) + deltaX, 1.5f * (gridSize + (gridSize - 2 - i)) );
                GameObject tileGameObject = Instantiate(hexTile, new Vector3(pos.x, 0, pos.y), Quaternion.identity);
                tileGameObject.name = $"Tile_{gridSize + (gridSize - 2 - i)}_{x}";

                tileGameObject.transform.parent = transform;
                
                Tile newTile = tileGameObject.AddComponent<Tile>();
                newTile.position = pos;
                newTile.type = "grass";

                if( x != 0) {
                    AddNeighbourAtIndex(tiles.Count - 1, newTile);
                }
                AddNeighbourAtIndex(tiles.Count - 1 - x - (gridSize + i - 1) + x - 1, newTile);
                
                AddNeighbourAtIndex(tiles.Count - 1 - x - (gridSize + i - 1) + x, newTile);
                

                tiles.Add(newTile);
            }
            deltaX += Mathf.Sqrt(3) / 2;
        }
    }

    void AddNeighbourAtIndex(int index, Tile tile)
    {
        if (index >= 0 && index < tiles.Count && !tile.neighbors.Contains(tiles[index]))
        {
            tile.neighbors.Add(tiles[index]);
            tiles[index].neighbors.Add(tile);
        }
        else
        {
            Debug.LogWarning($"Index {index} is out of bounds for tiles list.");
        }
    }
}
