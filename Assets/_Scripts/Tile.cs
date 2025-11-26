using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem.XR.Haptics;

public class Tile : MonoBehaviour
{
    public List<Tile> neighbors = new List<Tile>();

    public bool hasCity = false;
    public bool hasMountains = false;
    public bool hasForest = false;

    public float humidity;
    public float temperature;
    public float height;

    public string terrain;

    public Vector2 position;
    public Player owner;
    public Unit unit;
    public City city;
    public City underCity;

    public float generationTimer = 0;
    public float generationTimer1 = 0;

    void Update()
    {
        if (underCity != null)
        {
            generationTimer += Time.deltaTime;
            generationTimer1 += Time.deltaTime;
            if (!hasMountains && !hasForest)
            {
                int generated = (int)Mathf.Floor(generationTimer / Global.timePerCoinPerTile);
                
                generationTimer -= generated * Global.timePerCoinPerTile;
                SendToCity(generated, 0, 0);
            }
            else if (hasMountains && hasForest)
            {
                int generatedWood = (int)Mathf.Floor(generationTimer / Global.timePerLogPerForest);
                generationTimer -= generatedWood * Global.timePerLogPerForest;
                int generatedStone = (int)Mathf.Floor(generationTimer1 / Global.timePerStonePerMountain);
                generationTimer1 -= generatedStone * Global.timePerStonePerMountain;
                SendToCity(0, generatedWood, generatedStone);
            }
            else if (hasForest)
            {
                int generated = (int)Mathf.Floor(generationTimer / Global.timePerLogPerForest);
                generationTimer -= generated * Global.timePerLogPerForest;
                SendToCity(0, generated, 0);
            }
            else if (hasMountains)
            {
                int generated = (int)Mathf.Floor(generationTimer / Global.timePerStonePerMountain);
                generationTimer -= generated * Global.timePerStonePerMountain;
                SendToCity(0, 0, generated);
            }
        }
    }

    public void ApplyTerrain(Vector3 newTerrain)
    {
        humidity = newTerrain.x;
        temperature = newTerrain.y;
        height = newTerrain.z;

        GameObject mountain = null;
        if (Mathf.Pow(height * height * (1-temperature), 0.33333f) > Global.heightToMountain)
        {
            mountain = Instantiate(Global.mountainPrefab, transform.position, Quaternion.identity);
            mountain.transform.SetParent(transform);
            hasMountains = true;
        }

        GameObject forest = null;
        if (Mathf.Pow(humidity * humidity * temperature, 0.33333f) > Global.hotWetnessToForest)
        {
            forest = Instantiate(Global.forestPrefab, transform.position, Quaternion.identity);
            forest.transform.SetParent(transform);
            hasForest = true;
        }

        int temperatureInt = Mathf.FloorToInt(temperature * 4f - 0.001f +0.3f);
        int humidityInt = Mathf.FloorToInt(humidity * 5f - 0.001f);
        if (temperatureInt < 0)
        {
            temperatureInt = 0;
        }
        if (humidityInt < 0){ 
            humidityInt = 0; 
        }
        if (temperatureInt > 3)
        {
            temperatureInt = 3;
        }
        if (humidityInt > 4)
        {
            humidityInt = 4;
        }

        terrain = Global.terrainTypes[temperatureInt, humidityInt];

        foreach(Material mat in Global.terrainMaterials)
        {
            if (mat.name == terrain)
            {
                GetComponent<Renderer>().material = mat;
                if (mountain != null)
                {
                    mountain.GetComponent<Renderer>().material = mat;
                }
                if (forest != null)
                {
                    Renderer rend = forest.GetComponent<Renderer>();
                    rend.material = mat;
                }
                break;
            }
        }

        /*terrainType = (int)Mathf.Floor(terrain * 4f);
        GetComponent<Renderer>().material = Global.terrainMaterials[terrainType];*/
    }

    void SendToCity(int money, int wood, int stone)
    {
        underCity.RecieveResources(money, wood, stone);
    }
}
