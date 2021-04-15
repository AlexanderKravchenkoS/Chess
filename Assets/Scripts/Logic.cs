using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Logic
{
    public static bool isCorrectMove
        (Figure[,] figuresOnBoard, Figure selectedFigure, Vector2Int finalPosition)
    {
        if (finalPosition.x < 0 || finalPosition.x > 7
            || finalPosition.y < 0 || finalPosition.y > 7)
        {
            return false;
        }

        if (finalPosition.x == selectedFigure.figureData.x
            && finalPosition.y == selectedFigure.figureData.y)
        {
            return false;
        }

        if (figuresOnBoard[finalPosition.x, finalPosition.y] != null)
        {
            if (selectedFigure.figureData.isWhite ==
                figuresOnBoard[finalPosition.x, finalPosition.y].figureData.isWhite)
            {
                return false;
            }
        }

        bool result = false;
        int deltaX = Mathf.Abs(finalPosition.x - selectedFigure.figureData.x);
        int deltaY = Mathf.Abs(finalPosition.y - selectedFigure.figureData.y);
        switch (selectedFigure.figureData.type)
        {
            case Type.Bishop:
                if (deltaX == deltaY)
                {
                    bool figureOnWay = false;
                    int xMax = Mathf.Max(finalPosition.x, selectedFigure.figureData.x);
                    int yMax = Mathf.Max(finalPosition.y, selectedFigure.figureData.y);
                    int xMin = Mathf.Min(finalPosition.x, selectedFigure.figureData.x);
                    int yMin = Mathf.Min(finalPosition.y, selectedFigure.figureData.y);

                    for (int i = xMin + 1; i < xMax; i++)
                    {
                        for (int j = yMin + 1; j < yMax; j++)
                        {
                            if (i - xMin == j - yMin && figuresOnBoard[i, j] != null)
                            {
                                figureOnWay = true;
                            }
                        }
                    }

                    if (figureOnWay)
                    {
                        result = false;
                    }
                    else
                    {
                        result = true;
                    }
                }
                break;

            case Type.King:
                if ((deltaX == 1 && deltaY == 1)
                    || (deltaX == 1 && deltaY == 0)
                    || (deltaX == 0 && deltaY == 1))
                {
                    result = true;
                }
                else if (deltaX == 2 && deltaY == 0 && selectedFigure.figureData.turnCount == 0)
                {
                    result = true;
                }
                break;

            case Type.Knight:
                if ((deltaX == 1 && deltaY == 2) || (deltaX == 2 && deltaY == 1))
                {
                    result = true;
                }
                break;

            case Type.Pawn:
                int direction;
                if (selectedFigure.figureData.isWhite)
                {
                    direction = 1;
                }
                else
                {
                    direction = -1;
                }
                if (finalPosition.y - selectedFigure.figureData.y == direction * 2
                    && deltaX == 0 && selectedFigure.figureData.turnCount == 0
                    && figuresOnBoard[finalPosition.x, finalPosition.y] == null)
                {
                    result = true;
                }
                else if (finalPosition.y - selectedFigure.figureData.y == direction
                    && deltaX == 0 && figuresOnBoard[finalPosition.x, finalPosition.y] == null)
                {
                    result = true;
                }
                else if (finalPosition.y - selectedFigure.figureData.y == direction
                    && deltaX == 1)
                {
                    if (figuresOnBoard[finalPosition.x, finalPosition.y] != null
                        && figuresOnBoard[finalPosition.x, finalPosition.y].figureData.isWhite
                        != selectedFigure.figureData.isWhite)
                    {
                        result = true;
                    }
                    else if (figuresOnBoard[finalPosition.x, selectedFigure.figureData.y] != null
                        && figuresOnBoard[finalPosition.x, selectedFigure.figureData.y]
                        .figureData.isWhite != selectedFigure.figureData.isWhite)
                    {
                        result = true;
                    }
                }
                break;

            case Type.Queen:
                if (deltaX == 0)
                {
                    bool figureOnWay = false;
                    int yMax = Mathf.Max(finalPosition.y, selectedFigure.figureData.y);
                    int yMin = Mathf.Min(finalPosition.y, selectedFigure.figureData.y);

                    for (int j = yMin + 1; j < yMax; j++)
                    {
                        if (figuresOnBoard[finalPosition.x, j] != null)
                        {
                            figureOnWay = true;
                        }
                    }

                    if (figureOnWay)
                    {
                        result = false;
                    }
                    else
                    {
                        result = true;
                    }
                }

                else if (deltaY == 0)
                {
                    bool figureOnWay = false;
                    int xMax = Mathf.Max(finalPosition.x, selectedFigure.figureData.x);
                    int xMin = Mathf.Min(finalPosition.x, selectedFigure.figureData.x);

                    for (int i = xMin + 1; i < xMax; i++)
                    {
                        if (figuresOnBoard[i, finalPosition.y] != null)
                        {
                            figureOnWay = true;
                        }
                    }

                    if (figureOnWay)
                    {
                        result = false;
                    }
                    else
                    {
                        result = true;
                    }
                }

                else if (deltaX == deltaY)
                {
                    bool figureOnWay = false;
                    int xMax = Mathf.Max(finalPosition.x, selectedFigure.figureData.x);
                    int yMax = Mathf.Max(finalPosition.y, selectedFigure.figureData.y);
                    int xMin = Mathf.Min(finalPosition.x, selectedFigure.figureData.x);
                    int yMin = Mathf.Min(finalPosition.y, selectedFigure.figureData.y);

                    for (int i = xMin + 1; i < xMax; i++)
                    {
                        for (int j = yMin + 1; j < yMax; j++)
                        {
                            if (i - xMin == j - yMin && figuresOnBoard[i, j] != null)
                            {
                                figureOnWay = true;
                            }
                        }
                    }

                    if (figureOnWay)
                    {
                        result = false;
                    }
                    else
                    {
                        result = true;
                    }
                }
                else
                {
                    result = false;
                }
                break;

            case Type.Rook:
                if (deltaX == 0)
                {
                    bool figureOnWay = false;
                    int yMax = Mathf.Max(finalPosition.y, selectedFigure.figureData.y);
                    int yMin = Mathf.Min(finalPosition.y, selectedFigure.figureData.y);

                    for (int j = yMin + 1; j < yMax; j++)
                    {
                        if (figuresOnBoard[finalPosition.x, j] != null)
                        {
                            figureOnWay = true;
                        }
                    }

                    if (figureOnWay)
                    {
                        result = false;
                    }
                    else
                    {
                        result = true;
                    }
                }
                else if (deltaY == 0)
                {
                    bool figureOnWay = false;
                    int xMax = Mathf.Max(finalPosition.x, selectedFigure.figureData.x);
                    int xMin = Mathf.Min(finalPosition.x, selectedFigure.figureData.x);

                    for (int i = xMin + 1; i < xMax; i++)
                    {
                        if (figuresOnBoard[i, finalPosition.y] != null)
                        {
                            figureOnWay = true;
                        }
                    }

                    if (figureOnWay)
                    {
                        result = false;
                    }
                    else
                    {
                        result = true;
                    }
                }
                break;
        }
        return result;
    }

    public static bool isNeedToDestroy
        (Figure[,] figuresOnBoard, Figure selectedFigure, Vector2Int finalPosition, out Vector2Int destroyPosition)
    {
        if (figuresOnBoard[finalPosition.x, finalPosition.y] != null)
        {
            destroyPosition = finalPosition;
            return true;
        }
        if (selectedFigure.figureData.type == Type.Pawn
            && Mathf.Abs(finalPosition.x - selectedFigure.figureData.x) == 1)
        {
            destroyPosition = new Vector2Int(finalPosition.x, selectedFigure.figureData.y);
            return true;
        }
        destroyPosition = -Vector2Int.one;
        return false;
    }
}
