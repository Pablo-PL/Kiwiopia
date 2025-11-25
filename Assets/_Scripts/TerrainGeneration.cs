using UnityEngine;

public class TerrainGeneration : MonoBehaviour
{
    [SerializeField] private int humidityXOffset;
    [SerializeField] private int humidityZOffset;

    [SerializeField] private int temperatureXOffset;
    [SerializeField] private int temperatureZOffset;

    [SerializeField] private int heightXOffset;
    [SerializeField] private int heightZOffset;

    [SerializeField] private float humidityXScale;
    [SerializeField] private float humidityZScale;

    [SerializeField] private float temperatureXScale;
    [SerializeField] private float temperatureZScale;

    [SerializeField] private float heightXScale;
    [SerializeField] private float heightZScale;
    public void GenerateOffset()
    {
        humidityXOffset = Random.Range(100, 1000);
        humidityZOffset = Random.Range(100, 1000);
        temperatureXOffset = Random.Range(100, 1000);
        temperatureZOffset = Random.Range(100, 1000);
        heightXOffset = Random.Range(100, 1000);
        heightZOffset = Random.Range(100, 1000);
    }

    public Vector3 GetTerrainAtPos(float posX, float posZ)
    {
        
        return new Vector3( Mathf.PerlinNoise(humidityXScale * (humidityXOffset + posX), humidityZScale * (humidityZOffset + posZ)),
                            Mathf.PerlinNoise(temperatureXScale * (temperatureXOffset + posX), temperatureZScale * (temperatureZOffset + posZ)),
                            Mathf.PerlinNoise(heightXScale * (heightXOffset + posX), heightZScale * (heightZOffset + posZ))
                            );
    }
}
