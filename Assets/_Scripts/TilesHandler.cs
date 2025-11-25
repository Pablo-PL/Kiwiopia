using System.Collections.Generic;
using UnityEngine;



public class TilesHandler : MonoBehaviour
{
    public int gridSize;
    public GameObject hexTile;
    public List<Tile> tiles = new List<Tile>();
    public GameObject centerTile;


    [SerializeField] private TerrainGeneration terrainGeneration;

    private Tile getNewTile(Vector2 pos, int i, int x)
    {
        GameObject tileGameObject = Instantiate(hexTile, new Vector3(pos.x, 0, pos.y), Quaternion.identity);
        tileGameObject.name = $"Tile_{i}_{x}";
        tileGameObject.transform.parent = transform;
        Tile newTile = tileGameObject.AddComponent<Tile>();
        newTile.position = pos;
        return newTile;
    }

    void Start()
    {
        if (gridSize % 2 == 0)
        {
            gridSize += 1;
        }

        terrainGeneration.GenerateOffset();

        Vector2 startingPosition = new Vector2((-Mathf.Sqrt(3)/2) * (gridSize - 1), (-gridSize + 1) * 1.5f);
        float deltaX = 0f;
        for (int i = 0; i < gridSize; i++)
        {
            for (int x = 0; x < gridSize + i; x++)
            {
                Vector2 pos = startingPosition + new Vector2(x * Mathf.Sqrt(3) + deltaX, 1.5f * i);

                Tile newTile = getNewTile(pos, i, x);

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


                if (i == gridSize - 1 && x == (gridSize + i - 1)/2)
                {
                    centerTile = newTile.gameObject;
                }
            }
            deltaX -= Mathf.Sqrt(3) / 2;

            
        }
        deltaX += Mathf.Sqrt(3);
        for (int i = gridSize - 2; i >= 0; i--)
        {
            for (int x = 0; x < gridSize + i; x++)
            {
                Vector2 pos = startingPosition + new Vector2(x * Mathf.Sqrt(3) + deltaX, 1.5f * (gridSize + (gridSize - 2 - i)) );
                Tile newTile = getNewTile(pos, 2 * gridSize - i - 2, x);


                if ( x != 0) {
                    AddNeighbourAtIndex(tiles.Count - 1, newTile);
                }
                AddNeighbourAtIndex(tiles.Count - 1 - x - (gridSize + i - 1) + x - 1, newTile);
                
                AddNeighbourAtIndex(tiles.Count - 1 - x - (gridSize + i - 1) + x, newTile);
                

                tiles.Add(newTile);
            }
            deltaX += Mathf.Sqrt(3) / 2;
        }

        foreach (Tile tile in tiles)
        {
            tile.ApplyTerrain(terrainGeneration.GetTerrainAtPos(tile.transform.position.x, tile.transform.position.z));
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

    public List<Tile> shortestPath(Tile source, Tile end)
    {
        if (source == null || end == null)
            return new List<Tile>();

        // BFS kolejka
        Queue<Tile> queue = new Queue<Tile>();
        queue.Enqueue(source);

        // S³ownik przechowuj¹cy poprzednika ka¿dego odwiedzonego wêz³a
        Dictionary<Tile, Tile> cameFrom = new Dictionary<Tile, Tile>();
        cameFrom[source] = null;

        while (queue.Count > 0)
        {
            Tile current = queue.Dequeue();

            if (current == end)
            {
                break; // znaleziono cel
            }

            foreach (Tile neighbor in current.neighbors)
            {
                if (!cameFrom.ContainsKey(neighbor) && !neighbor.hasMountains)
                {
                    cameFrom[neighbor] = current;
                    queue.Enqueue(neighbor);
                }
            }
        }

        // Odtworzenie œcie¿ki od end do source
        List<Tile> path = new List<Tile>();
        Tile temp = end;
        while (temp != null)
        {
            path.Add(temp);
            cameFrom.TryGetValue(temp, out temp);
        }

        path.Reverse(); // od source do end
        if (path.Count > 0 && path[0] != source)
        {
            // brak po³¹czenia miêdzy source a end
            return new List<Tile>();
        }

        return path;
    }
}
