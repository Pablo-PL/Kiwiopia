using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SelectionHandler : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private CameraMovement cameraMovement;
    [SerializeField] private UIHandler uIHandler;
    [SerializeField] private UnitsHandler unitsHandler;
    [SerializeField] private TilesHandler tilesHandler;

    public int state = 0; 
    //0 - none; 1 - tile; 2 - city; 3 - unit

    public Tile lastClickedTile;
    private int howManyTimesClickedSame;

    private InputActionMap selectionMap;
    private InputAction select;
    private InputAction mousePosition;

    private bool clicked = false;

    void Start()
    {
        howManyTimesClickedSame = 0;
        selectionMap = InputSystem.actions.FindActionMap("Selection");
        select = selectionMap.FindAction("Select");
        mousePosition = selectionMap.FindAction("Mouse Position");
        select.performed += ctx => clicked = true;
    }

    void OnSelect()
    {
        Tile tile = getTileOnMouse();
        if (tile == null)
        {
            return;
        }
       
        if (tile == lastClickedTile)
        {
            howManyTimesClickedSame++;
            //////TO-DO
        }
        else
        {
            howManyTimesClickedSame = 0;
            if (state == 3)
            {
                lastClickedTile.unit.MoveUnit(tilesHandler.shortestPath(lastClickedTile, tile));
                state = 0;
                uIHandler.ClickedTile(lastClickedTile, 0);
                return;
            }
        }

        uIHandler.ClickedTile(tile, howManyTimesClickedSame);

        cameraMovement.UpdateFocusPoint(tile.transform);
        lastClickedTile = tile;   
    }

    public Tile getTileOnMouse()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return null;
        }
        Vector2 mousePos = mousePosition.ReadValue<Vector2>();
        Ray ray = mainCamera.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Tile tile = hit.collider.GetComponent<Tile>();
            if (tile != null)
            {
                return tile;
            }
        }
        return null;
    }

    void Update()
    {
        if (clicked)
        {
            OnSelect();
            clicked = false;
        }
    }
}
