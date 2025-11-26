using UnityEngine;

public class Player : MonoBehaviour
{
    public int hash;
    public int money;
    public int wood;
    public int stone;

    [SerializeField] private TilesHandler tilesHandler;
    [SerializeField] private UnitsHandler unitsHandler;
    [SerializeField] private CameraMovement cameraMovement;
    [SerializeField] private UIHandler uIHandler;

    void Awake()
    {
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
                startingUnit = unitsHandler.RecruitUnit(startingTile);
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
    }

    void SendValuesToUI()
    {
        uIHandler.UpdateMoneyText(money);
        uIHandler.UpdateWoodText(wood);
        uIHandler.UpdateStoneText(stone);
    }

    void Update()
    {
        SendValuesToUI();
    }

    public void RecieveResources(int rMoney, int rWood, int rStone)
    {
        money += rMoney;
        wood += rWood;
        stone += rStone;
    }
}
