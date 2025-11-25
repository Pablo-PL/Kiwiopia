
using UnityEngine;
using System.Collections.Generic;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private CityHandler cityHandler;
    [SerializeField] private UnitsHandler unitsHandler;
    [SerializeField] private SelectionHandler selectionHandler;

    [SerializeField] private RectTransform tileMenu;
    [SerializeField] private RectTransform cityMenu;
    [SerializeField] private RectTransform unitMenu;

    private List<KeyValuePair<int, RectTransform>> menus = new List<KeyValuePair<int, RectTransform>>();

    private void Start()
    {
        DisableAll();
    }
    public void ClickedTile(Tile tile, int num_of_times)
    {
        if (tile == null) return;
        DisableAll();

        menus.Clear();
        menus.Add(new KeyValuePair<int, RectTransform>(1 ,tileMenu));
        if (tile.city != null) menus.Add(new KeyValuePair<int, RectTransform>(2, cityMenu));
        if (tile.unit != null) menus.Add(new KeyValuePair<int, RectTransform>(3, unitMenu));

        ActivateMenu(num_of_times);
        //Debug.Log($"Clicked tile: {tile.transform.name}, {num_of_times} times");
    }

    private void DisableAll()
    {
        cityMenu.gameObject.SetActive(false);
        unitMenu.gameObject.SetActive(false);
        tileMenu.gameObject.SetActive(false);
    }

    private void ActivateMenu(int num_of_times)
    {
        int menu_index = num_of_times % (menus.Count + 1);
        if (menu_index < menus.Count) {
            menus[menu_index].Value.gameObject.SetActive(true);
            selectionHandler.state = menus[menu_index].Key;
        }
        else
        {
            selectionHandler.state = 0;
        }
    }


    public void BuildCity()
    {
        cityHandler.BuildCity(selectionHandler.lastClickedTile);
    }

    public void RecruitUnit()
    {
        unitsHandler.RecruitUnit(selectionHandler.lastClickedTile);
    }

    public void DestroyUnit()
    {
        unitsHandler.DestroyUnit(selectionHandler.lastClickedTile);
    }
}
