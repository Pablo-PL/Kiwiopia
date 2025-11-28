using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Netcode;

public class Player : NetworkBehaviour
{
    public int hash;
    public int money;
    public int wood;
    public int stone;

    [SerializeField] private TilesHandler tilesHandler;
    [SerializeField] private UnitsHandler unitsHandler;
    [SerializeField] private CameraMovement cameraMovement;
    [SerializeField] private UIHandler uIHandler;

    public List<Unit> units = new List<Unit>();
    public List<City> citys = new List<City>();


    override public void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        tilesHandler = GameObject.Find("Tiles").GetComponent<TilesHandler>();
        unitsHandler = GameObject.Find("Units").GetComponent<UnitsHandler>();
        cameraMovement = GameObject.Find("Main Camera").GetComponent<CameraMovement>();
        uIHandler = Global.UIHandler;

        hash = Random.Range(0, 1000000000);

        money = Global.startingMoney;
        wood = Global.startingWood;
        stone = Global.startingStone;

        SendValuesToUI();

        bool foundStartingTile = false;
        Tile startingTile = null;
        Unit startingUnit = null;
        for (int i = 0; i < 100; i++)
        {
            startingTile = tilesHandler.RandomTile();
            if (startingTile.unit == null && startingTile.city == null && startingTile.owner == null && !startingTile.hasMountains)
            {
                foundStartingTile = true;
                startingUnit = unitsHandler.RecruitUnit(this, startingTile, 0);
                startingUnit.isLeader = true;
                startingTile.owner = this;
                startingUnit.owner = this;
                break;
            }
        }
        if (!foundStartingTile)
        {
            Debug.Log("Couldnt find starting tile");
            Destroy(gameObject);
        }
        cameraMovement.UpdateFocusPoint(startingTile.transform);
        UpdateVisiblitity();

    }

    void SendValuesToUI()
    {
        uIHandler.UpdateMoneyText(money);
        uIHandler.UpdateWoodText(wood);
        uIHandler.UpdateStoneText(stone);
    }

    void Update()
    {
        if (!IsOwner) return;
        SendValuesToUI();
    }

    public void RecieveResources(int rMoney, int rWood, int rStone)
    {
        money += rMoney;
        wood += rWood;
        stone += rStone;
    }



    public bool TakeResources(int tMoney, int tWood, int tStone)
    {
        if (money >= tMoney && wood >= tWood && stone >= tStone)
        {
            money -= tMoney;
            wood -= tWood;
            stone -= tStone;
            return true;
        }
        Debug.Log($"Not enouch resources: {tMoney}, {tWood}, {tStone}");
        return false;
    }

    public void AddUnit(Unit unit)
    {
        units.Add(unit);
    }
    public void RemoveUnit(Unit unit)
    {
        units.Remove(unit);
    }

    public void UpdateVisiblitity()
    {
        List<Tile> visibleTiles = new List<Tile>();

        foreach (City city in citys)
        {
            if (!visibleTiles.Contains(city.tile)) visibleTiles.Add(city.tile);
            visibleTiles.AddRange(city.cityTiles.Where(x=>!visibleTiles.Contains(x)));
        }

        foreach (Unit unit in units)
        {
            if (unit.tile != null)
            {
                List<Tile> unitVisibleTiles = new List<Tile>();
                unitVisibleTiles.Add(unit.tile);
                for (int i = 0; i < Global.unitTypes[unit.type].scoutDistance; i++)
                {
                    int visTiles = unitVisibleTiles.Count;
                    for (int j = 0; j < visTiles; j++)
                    {
                        Tile tile = unitVisibleTiles[j];
                        foreach (Tile neighbour in tile.neighbors)
                        {
                            if (!unitVisibleTiles.Contains(neighbour))
                            {
                                unitVisibleTiles.Add(neighbour);
                            }
                        }
                    }
                }
                visibleTiles.AddRange(unitVisibleTiles.Where(x=>!visibleTiles.Contains(x)));
            }
        }

        tilesHandler.SetVisibility(visibleTiles);
    }
}
