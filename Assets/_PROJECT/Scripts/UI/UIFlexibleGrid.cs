using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFlexibleGrid : LayoutGroup
{   
    public enum FitType
    {
        UNIFORM,
        WIDTH,
        HEIGHT,
        FIXEDROWS,
        FIXEDCOLUMNS
    }

    [Header("Cell Settings")]
    public FitType fitType;
    public int rows;
    public int columns;
    public Vector2 cellSize;
    public bool fitX;
    public bool fitY;

    [Header("Grid Settings")]
    public Vector2 spacing;
    public bool centerLastRow;
    public bool autoRowCount;

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();

        if (autoRowCount) rows = rectChildren.Count / columns + 1;

        if (rows <= 0) rows = 1;
        if (columns <= 0) columns = 1;

        if (fitType == FitType.WIDTH || fitType == FitType.HEIGHT || fitType == FitType.UNIFORM)
        {
            float sqrRt = Mathf.Sqrt(transform.childCount);
            rows = Mathf.CeilToInt(sqrRt);
            columns = Mathf.CeilToInt(sqrRt);
        }

        if (fitType == FitType.WIDTH)
        {
            fitX = true;
            rows = Mathf.CeilToInt(transform.childCount / (float)columns);
        }
        else if (fitType == FitType.HEIGHT)
        {
            fitY = true;
            columns = Mathf.CeilToInt(transform.childCount / (float)rows);
        }
        else if (fitType == FitType.UNIFORM)
        {
            fitX = true;
            fitY = true;
        }

        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;

        float cellWidth = (parentWidth / (float)columns) - ((spacing.x / (float)columns) * (columns - 1)) - (padding.left / (float)columns) - (padding.right / (float)columns);
        float cellHeight = (parentHeight / (float)rows) - ((spacing.y / (float)rows) * (rows - 1)) - (padding.top / (float)rows) - (padding.bottom / (float)rows);

        cellSize.x = fitX ? cellWidth : cellSize.x;
        cellSize.y = fitX ? cellHeight : cellSize.y;

        int columnCount = 0;
        int rowCount = 0;

        float childrenWidth = 0f;
        if (columns > 1) childrenWidth = (columns * cellSize.x) + ((columns - 1) * spacing.x);
        else childrenWidth = (columns * cellSize.x);

        float childrenHeight = 0f;
        if (rows > 1) childrenHeight = (rows * cellSize.y) + ((rows - 1) * spacing.y);
        else childrenHeight = (rows * cellSize.y);
        
        float centerXOffset = (parentWidth - padding.left - padding.right - childrenWidth) * 0.5f;
        float centerYOffset = (parentHeight - padding.top - padding.bottom - childrenHeight) * 0.5f;

        int lastRowCount = rectChildren.Count % columns;
        float lastRowWidth = 0f;
        float lastRowOffset = 0f;
        if (lastRowCount > 0)
        {
            if (lastRowCount > 1) lastRowWidth = (lastRowCount * cellSize.x) + ((lastRowCount - 1) * spacing.x);
            else lastRowWidth = (lastRowCount * cellSize.x);

            lastRowOffset = (parentWidth - padding.left - padding.right - lastRowWidth) * 0.5f;
            lastRowOffset -= centerXOffset;
            print(lastRowOffset);
        }

        for (int i = 0; i < rectChildren.Count; i++)
        {
            rowCount = i / columns;
            columnCount = i % columns;

            var item = rectChildren[i];

            float xPos = 0f;
            float yPos = 0f;
            switch (childAlignment)
            {
                case TextAnchor.UpperLeft:
                    xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;
                    yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top;
                    break;

                case TextAnchor.UpperCenter:
                    xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left + centerXOffset;
                    yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top;

                    if (centerLastRow && rowCount == rows - 1)
                    {
                        xPos += lastRowOffset;
                    }
                    break;

                case TextAnchor.UpperRight:
                    xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.right + (centerXOffset * 2f);
                    yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top;
                    break;

                case TextAnchor.MiddleLeft:
                    xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;
                    yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top + centerYOffset;
                    break;

                case TextAnchor.MiddleCenter:
                    xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left + centerXOffset;
                    yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top + centerYOffset;
                    break;

                case TextAnchor.MiddleRight:
                    xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.right + (centerXOffset * 2f);
                    yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top + centerYOffset;
                    break;

                case TextAnchor.LowerLeft:
                    xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;
                    yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top + (centerYOffset * 2f);
                    break;

                case TextAnchor.LowerCenter:
                    xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left + centerXOffset;
                    yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top + (centerYOffset * 2f);
                    break;

                case TextAnchor.LowerRight:
                    xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left + (centerXOffset * 2f);
                    yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top + (centerYOffset * 2f);
                    break;
            }
            //xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;
            //yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top;

            SetChildAlongAxis(item, 0, xPos, cellSize.x);
            SetChildAlongAxis(item, 1, yPos, cellSize.y);
        }
    }

    public override void CalculateLayoutInputVertical()
    {
        //base.CalculateLayoutInputVertical();
    }

    public override void SetLayoutHorizontal()
    {
        //throw new System.NotImplementedException();
    }

    public override void SetLayoutVertical()
    {
        //throw new System.NotImplementedException();
    }
}
