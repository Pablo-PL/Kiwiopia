using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class UnitsHandler : MonoBehaviour
{
    [SerializeField] private GameObject unitPrefab;

    [SerializeField] private TilesHandler tilesHandler;
    [SerializeField] private SelectionHandler selectionHandler;
    [SerializeField] private UIHandler uIHandler;

    [SerializeField] private LineRenderer lineRendererPrefab;
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


    public Unit RecruitUnit(Tile tile)
    {
        GameObject unitGameObj = Instantiate(unitPrefab, tile.transform.position, Quaternion.identity);
        Unit unit = unitGameObj.AddComponent<Unit>();
        unit.name = "speraman";
        tile.unit = unit;
        unit.tile = tile;
        unit.owner = tile.owner;
        unit.transform.parent = tile.transform;
        return unit;
    }

    public void DestroyUnit(Tile tile)
    {
        if (tile.unit != null)
        {
            Destroy(tile.unit.gameObject);
            Destroy(tile.unit);
            tile.unit = null;
        }
        if (tmpLineRenderer != null)
        {
            Destroy(tmpLineRenderer.gameObject);
        }
        selectionHandler.state = 0;
        uIHandler.ClickedTile(tile, 0);
    }

    
}
