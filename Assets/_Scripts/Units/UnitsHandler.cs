using System.Collections.Generic;
using UnityEngine;

public class UnitsHandler : MonoBehaviour
{
    [SerializeField] private GameObject unitPrefab;

    [SerializeField] private TilesHandler tilesHandler;
    [SerializeField] private SelectionHandler selectionHandler;
    [SerializeField] private UIHandler uIHandler;

    [SerializeField] private LineRenderer lineRendererPrefab;

    private List<Unit> units = new List<Unit>();
    private GameObject tmpLineRenderer;
    private Tile lastHoveredTile;
    
    void Update()
    {
        if (selectionHandler.state == 3)
        {
            Tile target = selectionHandler.getTileOnMouse();
            if (target != null)
            {
                if (lastHoveredTile == target)
                {
                    return;
                }

                if (tmpLineRenderer != null)
                {
                    Destroy(tmpLineRenderer);
                }
                
                List<Tile> path = tilesHandler.shortestPath(selectionHandler.lastClickedTile, target);
                LineRenderer newLineRenderer = Instantiate(lineRendererPrefab, Vector3.zero, Quaternion.Euler(90, 0, 0));
                newLineRenderer.numCornerVertices = 8;
                newLineRenderer.numCapVertices = 8;
                newLineRenderer.positionCount = path.Count;
                for (int i = 0; i < path.Count; i++)
                {
                    newLineRenderer.SetPosition(i, path[i].gameObject.transform.position + Vector3.up * 0.55f);
                }
                tmpLineRenderer = newLineRenderer.gameObject;
                lastHoveredTile = target;
            }
        }
        if (selectionHandler.state != 3 && tmpLineRenderer != null)
        {
            Destroy (tmpLineRenderer);
        }
    }


    public Unit RecruitUnit(Player owner, Tile tile, int unitType)
    {
        GameObject unitGameObj = Instantiate(Global.unitTypes[unitType].model, tile.transform.position, Quaternion.identity);
        Unit unit = unitGameObj.AddComponent<Unit>();
        owner.AddUnit(unit);
        unit.name = Global.unitTypes[unitType].name;
        unit.type = unitType;
        tile.unit = unit;
        unit.tile = tile;
        unit.owner = owner;
        unit.transform.parent = tile.transform;
        unit.unitsHandler = this;
        units.Add(unit);
        return unit;
    }

    public bool DestroyUnit(Unit unit)
    {
        if (unit.owner.units.Count <= 1)
        {
            return false;
        }
        unit.tile.unit = null;
        units.Remove(unit);
        unit.owner.RemoveUnit(unit);
        Destroy(unit.gameObject);
        Destroy(unit);
        
        if (tmpLineRenderer != null)
        {
            Destroy(tmpLineRenderer.gameObject);
        }
        selectionHandler.state = 0;
        return true;
    }
}
