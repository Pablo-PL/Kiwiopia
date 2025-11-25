using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{
    public static GameObject cityPrefab;
    [SerializeField] private GameObject cityPrefabReference;

    public static GameObject swordmanPrefab;
    [SerializeField] private GameObject swordmanPrefabReference;

    public static string[,] terrainTypes = {
        {"lodowiec",     "lodowiec",         "lodowiec",     "lodowiec",     "lodowiec" },
        {"sucha tundra", "mokra tundra",     "mokra tundra", "mokra tundra", "deszczowa tundra" },
        {"pustynia",     "step",             "suchy las",    "mokry las",    "las deszczowy"},
        {"pustynia",     "pustynne zaroœla", "suchy las",    "mokry las",    "las deszczowy"}
    };

    public static Material[] terrainMaterials;
    [SerializeField] private Material[] terrainMaterialReference;

    public static LineRenderer lineRendererPrefab;
    [SerializeField] private LineRenderer lineRendererPrefabReference;

    public static float heightToMountain;
    [SerializeField] private float heightToMoutainReference;

    public static GameObject mountainPrefab;
    [SerializeField] private GameObject mountainPrefabReference;

    public static float hotWetnessToForest;
    [SerializeField] private float hotWetnessToForestReference;

    public static GameObject forestPrefab;
    [SerializeField] private GameObject forestPrefabReference;

    public static Material completedLineMaterial;
    [SerializeField] private Material completedLineMaterialReference;


    public static Material inProgressLineMaterial;
    [SerializeField] private Material inProgressLineMaterialReference;

    void Awake()
    {
        cityPrefab = cityPrefabReference;
        swordmanPrefab = swordmanPrefabReference;
        terrainMaterials = terrainMaterialReference;
        lineRendererPrefab = lineRendererPrefabReference;
        heightToMountain = heightToMoutainReference;
        mountainPrefab = mountainPrefabReference;
        forestPrefab = forestPrefabReference;
        hotWetnessToForest = hotWetnessToForestReference;
        completedLineMaterial = completedLineMaterialReference;
        inProgressLineMaterial = inProgressLineMaterialReference;
    }
}
