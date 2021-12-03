using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonManager : MonoBehaviour
{
    bool active = false;
    GameController controller;

    public void ChangeStart_End()
    {
        controller = FindObjectOfType<GameController>();
        controller.enableTask = Enums.EnableTask.SelectStart;
        controller.tileGrid.ClearStart_End();
        controller.tileGrid.ResetGrid();
        controller.ui.Sync();
    }

    public void FindWay()
    {
        controller = FindObjectOfType<GameController>();
        if (controller.tileGrid.IsRunningState())
        {
            controller.tileGrid.AstarFindWay();
        }
    }

    void OnTriggerEnter2D()
    {
        controller = FindObjectOfType<GameController>();
        if (controller.enableTask == Enums.EnableTask.DrawWay)
        {
            // Only active when mouse is dragging
            if (controller.mouseDragging)
                controller.ObjectSelection(gameObject);
        }
    }

    public void SetActive(bool state)
    {
        this.active = state;
    }

    public bool GetActive()
    {
        return this.active;
    }

    public void ReloadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void EnableDrawBlock()
    {
        controller = FindObjectOfType<GameController>();
        controller.enableTask = Enums.EnableTask.DrawBlock;
        controller.clearStack = true;
        controller.ClearStack();

        controller.ValuesSync();
        controller.ui.Sync();
    }

    public void EnableDrawWay()
    {
        controller = FindObjectOfType<GameController>();
        controller.enableTask = Enums.EnableTask.DrawWay;

        controller.ValuesSync();
        controller.ui.Sync();
    }

    public void SaveMyDraw()
    {
        controller = FindObjectOfType<GameController>();
        TileData tileD = new TileData();
        tileD.blockList = new System.Collections.Generic.List<Vector2>();

        foreach(Tile tile in controller.tileGrid.Tiles)
        {
            if (controller.tileGrid.IsStart(tile))
                tileD.start_pos = new Vector2(tile.Row, tile.Col);
            else
            if (controller.tileGrid.IsEnd(tile))
                tileD.end_pos = new Vector2(tile.Row, tile.Col);

            if (tile.Weight == Values.TileWeight_Default)
                continue;
            else
                if (tile.Weight == Values.TileWeight_Expensive)
                tileD.blockList.Add(new Vector2(tile.Row, tile.Col));
        }

        // Create json file
        string json = JsonUtility.ToJson(tileD);
        string dataPath = Application.dataPath + "/Resources/Data/data.txt";

        File.AppendAllText(dataPath, json + "\n");
        
        
    }
}
