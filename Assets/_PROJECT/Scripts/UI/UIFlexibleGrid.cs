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

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();

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
                    xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;
                    yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top;
                    break;

                case TextAnchor.UpperRight:
                    break;

                case TextAnchor.MiddleLeft:
                    break;

                case TextAnchor.MiddleCenter:
                    break;

                case TextAnchor.MiddleRight:
                    break;

                case TextAnchor.LowerLeft:
                    break;

                case TextAnchor.LowerCenter:
                    break;

                case TextAnchor.LowerRight:
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
