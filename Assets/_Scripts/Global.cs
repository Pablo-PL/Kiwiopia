using UnityEngine;

public class Global : MonoBehaviour
{
    public static GameObject cityPrefab;
    [SerializeField] private GameObject cityPrefabReference;

    public static GameObject swordmanPrefab;
    [SerializeField] private GameObject swordmanPrefabReference;

    void Awake()
    {
        cityPrefab = cityPrefabReference;
        swordmanPrefab = swordmanPrefabReference;
    }
}
