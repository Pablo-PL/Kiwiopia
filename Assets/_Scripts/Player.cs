using UnityEngine;

public class Player : MonoBehaviour
{
    public int hash;
    [SerializeField] private TilesHandler tilesHandler;
    [SerializeField] private UnitsHandler unitsHandler;
    [SerializeField] private CameraMovement cameraMovement;

    void Awake()
    {


        hash = Random.Range(0, 1000000000);
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

    void Update()
    {
        
    }
}
