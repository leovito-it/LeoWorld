using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TileGrid))]
[RequireComponent(typeof(UIScript))]
public class GameController : MonoBehaviour
{
    [Header("TileGrid setup:")]
    [Range(1, 15)] public int column = 5;
    [Range(1, 20)] public int row = 5;
    [Range(1, 20)] public int spacing = 5;
    [Range(10, 100)] public float cellSize = 100f;
    [Header("Where to swapm tile:")]
    public Transform site;
    public Transform siteContainer;

    [Header("Tile setup:")]
    public GameObject template; // One object to build a tilemap

    [Header("Play mode")]
    public GameObject movableObject; // an object to show how to my stack working , can delete
    Vector2 movableObject_oldPos;  // can delete

    [SerializeField] private MyStack myStack;

    [SerializeField] [Range(1, 10)] float speed = 2f;
    [SerializeField] GameObject block;

    // Game checker
    public Enums.EnableTask enableTask = Enums.EnableTask.None;
    [HideInInspector] public bool mouseDragging = false;
    [HideInInspector] public bool clearStack = true;

    public TileGrid tileGrid;

    [Header("Find path")]
    public bool showSteps = false;
    public bool showPath = true;

    [HideInInspector] public UIScript ui;

    void Start()
    {
        ChangeSizeToFit();
        SetStartValues();

        UI_Setup();
        TileGrid_Setup();
        Components_Setup();

        ValuesSync();
    }

    //----- UI SETTINGS - START

    void SetStartValues()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        enableTask = Enums.EnableTask.SelectStart;
    }

    void UI_Setup()
    {
        ui = GetComponent<UIScript>();
        ui.SetGridLayoutGroup(site, column, cellSize, spacing);
        ui.Sync();
    }

    void TileGrid_Setup()
    {
        tileGrid = GetComponent<TileGrid>();
        TileGridSync();
        tileGrid.CreateTilemap(site);
    }

    void ChangeSizeToFit()
    {
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        Vector2 siteSize = screenSize + site.GetComponent<RectTransform>().sizeDelta; // only true with this game
        siteSize += siteContainer != null ? siteContainer.GetComponent<RectTransform>().sizeDelta : Vector2.zero;

        Debug.Log(siteSize.ToString());

        // change size = columns count
        if (column * (cellSize + spacing) != siteSize.x)
            cellSize = (siteSize.x / column) - spacing;
    }

    void Components_Setup()
    {
        if (movableObject != null)
            movableObject_oldPos = movableObject.transform.position;

        if (block != null)
            block.GetComponent<RectTransform>().sizeDelta = new Vector2(cellSize, cellSize);

        template.GetComponent<RectTransform>().sizeDelta = new Vector2(cellSize, cellSize);
        template.GetComponent<BoxCollider2D>().size = new Vector2(cellSize, cellSize);
    }

    //----- UI SETTINGS - FINISH
    public void ValuesSync()
    {
        Values.SHOW_PATH = showPath;
        Values.SHOW_STEPS = showSteps;
    }

    public void TileGridSync()
    {
        tileGrid.Rows = row;
        tileGrid.Cols = column;
        tileGrid.TilePrefab = template;
    }

    public void ObjectSelection(GameObject selected)
    {
        // Clear stack if mouse click again
        ClearStack();

        GameObject lastObj = myStack.GetLastObject();

        if (DistanceCalculator.IsSameAxis(selected, lastObj) &&
            DistanceCalculator.IsObjectInCircle(selected, lastObj, 1.5f * cellSize))
            myStack.Handle(selected);
    }

    public static void HandleTileClicked(Tile tile)
    {
        GameController controller = FindObjectOfType<GameController>();

        Debug.Log("HandleTileClicked, type = " + controller.enableTask.ToString());
        switch (controller.enableTask)
        {
            case Enums.EnableTask.DrawBlock:
                {
                    DrawBlock(controller, tile);
                    break;
                }
            case Enums.EnableTask.DrawWay:
                {

                    break;
                }
            case Enums.EnableTask.SelectStart:
                {
                    controller.tileGrid.SetStartPos(tile);
                    controller.enableTask = Enums.EnableTask.SelectEnd;
                    controller.ui.Sync();
                    break;
                }
            case Enums.EnableTask.SelectEnd:
                {
                    if (!controller.tileGrid.IsStart(tile))
                        controller.tileGrid.SetEndPos(tile);
                    controller.enableTask = Enums.EnableTask.ReadyForRunning;
                    controller.ui.Sync();
                    break;
                }
        }
    }

    public static void DrawBlock(GameController controller, Tile tile)
    {
        Vector2 pos = tile.ToVector2();

        if (tile.Weight != Values.TileWeight_Expensive)
        {
            Debug.Log("ADD");
            controller.tileGrid.CreateExpensive((int)pos.y, (int)pos.x);
            tile.GetGameObject().GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            Debug.Log("REMOVE");
            controller.tileGrid.RemoveExpensive((int)pos.y, (int)pos.x);
            tile.GetGameObject().GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    public void ClearStack()
    {
        if (clearStack)
        {
            myStack.Remove(myStack.Count());
            if (movableObject != null)
                movableObject.transform.position = movableObject_oldPos;
            clearStack = false;
        }
    }

    public void RunMovableObject()
    {
        if (movableObject == null)
            return;

        int currentTarget = 0;
        bool finish = false;

        // Set start position
        movableObject.transform.position = myStack.GetPositionOfObject(currentTarget);
        // Start
        StartCoroutine(Patrol(currentTarget, finish));
    }

    IEnumerator Patrol(int targetIndex, bool finish)
    {
        while (!finish)
        {
            if (targetIndex == myStack.Count() - 1)
            {
                finish = true;
                myStack.Remove(myStack.Count());
                break;
            }

            movableObject.transform.position += (Vector3)(myStack.GetPositionOfObject(targetIndex + 1) - myStack.GetPositionOfObject(targetIndex)).normalized * speed * Time.fixedDeltaTime;

            Debug.Log(myStack.GetPositionOfObject(targetIndex).ToString());
            if (Vector2.Distance(movableObject.transform.position, myStack.GetPositionOfObject(targetIndex + 1)) < 0.1f)
                targetIndex++;

            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }
}