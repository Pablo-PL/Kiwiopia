using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    public List<Tile> cityTiles = new List<Tile>();
    public Tile tile;
    public int size = 0;

    public int money = 0;
    public int wood = 0;
    public int stone = 0;

    public void ChangeSize(int newSize)
    {
        size = newSize;   
        foreach (Tile cityTile in cityTiles)
        {
            cityTile.underCity = null;
            cityTile.owner = null;
        }
        cityTiles.Clear();
        foreach(Tile neighbour in tile.neighbors)
        {
            if (neighbour.underCity == null && (neighbour.owner == tile.owner || neighbour.owner == null))
            {
                neighbour.underCity = this;
                cityTiles.Add(neighbour);
                neighbour.owner = tile.owner;
            }
        }
        for (int i = 0; i < newSize-1; i++)
        {
            int cityTilesCount = cityTiles.Count;
            for( int j = 0; j < cityTilesCount; j++) 
            {
                Tile cityTile = cityTiles[j];
                foreach (Tile neighbour in cityTile.neighbors)
                {
                    if (!cityTiles.Contains(neighbour) && neighbour.underCity == null && (neighbour.owner == tile.owner || neighbour.owner == null) && neighbour != tile)
                    {
                        neighbour.underCity = this;
                        cityTiles.Add(neighbour);
                        neighbour.owner = tile.owner;
                    }
                }
            }
        }
        foreach(Tile cityTile in cityTiles)
        {
            cityTile.transform.position = new Vector3(cityTile.transform.position.x, .15f, cityTile.transform.position.z);
        }
    }

    private void Update()
    {
        SendResourcesToPlayer();
    }

    void SendResourcesToPlayer()
    {
        if (money == 0 && wood == 0 && stone == 0)
        {
            return;
        }
        tile.owner.RecieveResources(money, wood, stone);
        money = 0;
        wood = 0;
        stone = 0;
    }

    public void RecieveResources(int rMoney, int rWood, int rStone)
    {
        money += rMoney;
        wood += rWood;
        stone += rStone;
    }
}
