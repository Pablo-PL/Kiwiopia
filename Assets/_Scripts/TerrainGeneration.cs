using TreeEditor;
using UnityEngine;

public class TerrainGeneration : MonoBehaviour
{
    [SerializeField] private int xOffset;
    [SerializeField] private int zOffset;

    [SerializeField] private float xScale;
    [SerializeField] private float zScale;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        xOffset = Random.Range(-10000, 10000);
        zOffset = Random.Range(-10000, 10000);
    }

    public float GetTerrainAtPos(Vector2 pos)
    {
        return Mathf.PerlinNoise(xScale * (xOffset + pos.x), zScale * (zOffset + pos.y));
    }
}
