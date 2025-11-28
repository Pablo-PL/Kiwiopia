
using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private PlayerHandler playerHandler;
    [SerializeField] private CityHandler cityHandler;
    [SerializeField] private UnitsHandler unitsHandler;
    [SerializeField] private SelectionHandler selectionHandler;

    [SerializeField] private RectTransform tileMenu;
    [SerializeField] private RectTransform cityMenu;
    [SerializeField] private RectTransform cityMenuUnits;

    [SerializeField] private RectTransform unitMenu;


    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text woodText;
    [SerializeField] private TMP_Text stoneText;

    private List<KeyValuePair<int, RectTransform>> menus = new List<KeyValuePair<int, RectTransform>>();

    private void Start()
    {
        DisableAll();
    }
    public void ClickedTile(Tile tile, int num_of_times)
    {
        if (tile.unit != null)
        {
            cityMenuUnits.gameObject.SetActive(false);
        }
        else
        {
            cityMenuUnits.gameObject.SetActive(true);
        }

        if (tile == null) return;
        DisableAll();

        menus.Clear();
        if (tile.unit != null) menus.Add(new KeyValuePair<int, RectTransform>(1 ,tileMenu));
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
        cityHandler.BuildCity(playerHandler.localPlayer ,selectionHandler.lastClickedTile);
    }

    public void RecruitUnit(int type)
    {
        unitsHandler.RecruitUnit(playerHandler.localPlayer ,selectionHandler.lastClickedTile, type);
        cityMenuUnits.gameObject.SetActive(false);
    }

    public void DestroyUnit()
    {
        if (unitsHandler.DestroyUnit(selectionHandler.lastClickedTile.unit))
        {
            ClickedTile(selectionHandler.lastClickedTile, 0);
        }
    }

    public void UpgradeCity()
    {
        selectionHandler.lastClickedTile.city.ChangeSize(selectionHandler.lastClickedTile.city.size + 1);
    }

    public void UpdateMoneyText(int value)
    {
        moneyText.text = value.ToString();
    }

    public void UpdateWoodText(int value)
    {
        woodText.text = value.ToString();
    }

    public void UpdateStoneText(int value)
    {
        stoneText.text = value.ToString();
    }
}
