using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    [Header("Notice")]
    public Text txtNotice;
    public Toggle showSteps;
    public Button enableDrawWay;
    public Button enableDrawBlock;
    public Button changeStartEnd;
    public Button findWay;

    GameController controller;

    void EnableButtons()
    {
        if (enableDrawWay != null)      enableDrawWay.enabled = true;
        if (enableDrawBlock != null)  enableDrawBlock.enabled = true;
        if (changeStartEnd != null)  changeStartEnd.enabled = true;
        if (findWay != null)  findWay.enabled = true;
    }

    public void SetButtonsState()
    {
        controller = GetComponent<GameController>();

        EnableButtons();
        switch (controller.enableTask)
        {
            case Enums.EnableTask.DrawBlock:
                {
                    if (enableDrawBlock != null)  enableDrawBlock.enabled = false;
                    break;
                }
            case Enums.EnableTask.DrawWay:
                {
                    if (enableDrawWay != null)  enableDrawWay.enabled = false;
                    break;
                }
            case Enums.EnableTask.SelectStart :
                {
                    if (findWay != null) findWay.enabled = false;
                    if (enableDrawBlock != null) enableDrawBlock.enabled = false;
                    if (enableDrawWay != null) enableDrawWay.enabled = false;
                    break;
                }
        }
    }

    public void SetGridLayoutGroup(Transform site, int column, float cellSize ,float spacing)
    {
        GridLayoutGroup glg = site.TryGetComponent(out glg) ? glg : site.gameObject.AddComponent<GridLayoutGroup>();

        glg.childAlignment = TextAnchor.MiddleCenter;
        glg.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        glg.spacing = new Vector2(spacing, spacing);
        glg.constraintCount = column; //when glg.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        glg.cellSize = new Vector2(cellSize, cellSize);
    }

    public void SetNoticeText()
    {
        controller = GetComponent<GameController>();
        string text = "";
        switch (controller.enableTask)
        {
            case Enums.EnableTask.DrawBlock:
                text = Values.NOTICE_DRAW_BLOCK;
                break;
            case Enums.EnableTask.DrawWay:
                text = Values.NOTICE_DRAW_WAY;
                break;
            case Enums.EnableTask.SelectStart:
                text = Values.NOTICE_SELECT_START_POS;
                break;
            case Enums.EnableTask.SelectEnd:
                text = Values.NOTICE_SELECT_END_POS;
                break;
            case Enums.EnableTask.ReadyForRunning:
                text = Values.NOTICE_CAN_RUNNING;
                break;
            default:
                text = "Nothing here";
                break;
        }
        txtNotice.text = text;
    }

    public void Sync()
    {
        SetButtonsState();
        SetNoticeText();
    }
}
