using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string type;
    public bool isMoving = false;
    public Player owner;
    public Tile tile;

    public void MoveUnit(List<Tile> path)
    {

        if (isMoving)
        {
            Debug.Log("unit already moving");
            return;
        }
        LineRenderer newLineRenderer = Instantiate(Global.lineRendererPrefab, Vector3.zero, Quaternion.Euler(90, 0, 0));
        newLineRenderer.numCornerVertices = 8;
        newLineRenderer.numCapVertices = 8;
        newLineRenderer.material = Global.inProgressLineMaterial;
        newLineRenderer.positionCount = path.Count;
        for (int i = 0; i < path.Count; i++)
        {
            newLineRenderer.SetPosition(i, path[i].gameObject.transform.position + Vector3.up * 0.55f);
        }
        newLineRenderer.transform.SetParent(transform, false);
        StartCoroutine(moveUnit(path, 1f, newLineRenderer));
    }

    private IEnumerator<Tile> moveUnit(List<Tile> path, float time, LineRenderer line)
    {
        LineRenderer completedLineRenderer = Instantiate(Global.lineRendererPrefab, Vector3.zero, Quaternion.Euler(90, 0, 0));
        completedLineRenderer.numCornerVertices = 8;
        completedLineRenderer.numCapVertices = 8;
        completedLineRenderer.positionCount = 1;
        completedLineRenderer.SetPosition(0, path[0].transform.position + Vector3.up * 0.56f);
        completedLineRenderer.material = Global.completedLineMaterial;
        tile.unit = null;
        transform.parent = null;
        for (int i = 0; i < path.Count - 1; i++)
        {
            Vector3 startPos = path[i].gameObject.transform.position;
            Vector3 destinationPos = path[i + 1].gameObject.transform.position;
            float timePassed = 0;
            while (timePassed < time)
            {
                gameObject.transform.position = Vector3.Lerp(startPos, destinationPos, timePassed / time);
                timePassed += Time.deltaTime;
                yield return null;
            }
            gameObject.transform.position = destinationPos;
            completedLineRenderer.positionCount = i + 2;
            completedLineRenderer.SetPosition(i + 1, path[i+1].transform.position + Vector3.up * 0.56f);
            line.positionCount = path.Count - i - 1;
            for (int j = 0; j < path.Count - i - 1; j++)
            {
                line.SetPosition(j, path[i + 1 + j].transform.position + Vector3.up * 0.55f);
            }
        }
        Destroy(line);
        Destroy(completedLineRenderer);
        isMoving = false;
        path[path.Count - 1].unit = GetComponent<Unit>();
        tile = path[path.Count - 1];
    }
}
