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
        bool figureOnWay = false;
        int deltaX = Mathf.Abs(finalPosition.x - selectedFigure.figureData.x);
        int deltaY = Mathf.Abs(finalPosition.y - selectedFigure.figureData.y);
        int xMax = Mathf.Max(finalPosition.x, selectedFigure.figureData.x);
        int yMax = Mathf.Max(finalPosition.y, selectedFigure.figureData.y);
        int xMin = Mathf.Min(finalPosition.x, selectedFigure.figureData.x);
        int yMin = Mathf.Min(finalPosition.y, selectedFigure.figureData.y);
        int derictionX = (int)Mathf.Sign(finalPosition.x - selectedFigure.figureData.x);
        int derictionY = (int)Mathf.Sign(finalPosition.y - selectedFigure.figureData.y);
        switch (selectedFigure.figureData.type)
        {
            case Type.Bishop:
                #region Bishop
                if (deltaX == deltaY)
                {
                    for (int i = selectedFigure.figureData.x + derictionX;
                        i < finalPosition.x; i += derictionX)
                    {
                        for (int j = selectedFigure.figureData.y + derictionY;
                            j < finalPosition.y; j += derictionY)
                        {
                            if (i - selectedFigure.figureData.x == j - selectedFigure.figureData.y && figuresOnBoard[i, j] != null)
                            {
                                figureOnWay = true;
                            }
                        }
                    }
                    result = !figureOnWay;
                }
                break;
            #endregion

            case Type.King:
                #region King
                if ((deltaX == 1 && deltaY == 1)
                    || (deltaX == 1 && deltaY == 0)
                    || (deltaX == 0 && deltaY == 1))
                {
                    result = true;
                }
                else if (deltaX == 2 && deltaY == 0 && selectedFigure.figureData.turnCount == 0
                    && figuresOnBoard
                    [finalPosition.x,(finalPosition.y - selectedFigure.figureData.y) / 2] == null)
                {
                    if (finalPosition.x == 3 && figuresOnBoard[0, finalPosition.y] != null)
                    {
                        if (figuresOnBoard[0, finalPosition.y].figureData.turnCount == 0)
                        {
                            result = true;
                        }
                    }
                    else if (finalPosition.x == 6 && figuresOnBoard[7, finalPosition.y] != null)
                    {
                        if (figuresOnBoard[7, finalPosition.y].figureData.turnCount == 0)
                        {
                            result = true;
                        }
                    }
                }
                break;
            #endregion

            case Type.Knight:
                #region Knight
                if ((deltaX == 1 && deltaY == 2) || (deltaX == 2 && deltaY == 1))
                {
                    result = true;
                }
                break;
            #endregion

            case Type.Pawn:
                #region Pawn
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
            #endregion

            case Type.Queen:
                #region Queen
                if (deltaX == 0)
                {
                    for (int j = yMin + 1; j < yMax; j++)
                    {
                        if (figuresOnBoard[finalPosition.x, j] != null)
                        {
                            figureOnWay = true;
                        }
                    }
                    result = !figureOnWay;
                }

                else if (deltaY == 0)
                {
                    for (int i = xMin + 1; i < xMax; i++)
                    {
                        if (figuresOnBoard[i, finalPosition.y] != null)
                        {
                            figureOnWay = true;
                        }
                    }
                    result = !figureOnWay;
                }

                else if (deltaX == deltaY)
                {
                    for (int i = selectedFigure.figureData.x + derictionX;
                        i < finalPosition.x; i += derictionX)
                    {
                        for (int j = selectedFigure.figureData.y + derictionY;
                            j < finalPosition.y; j += derictionY)
                        {
                            if (i - selectedFigure.figureData.x == j - selectedFigure.figureData.y && figuresOnBoard[i, j] != null)
                            {
                                figureOnWay = true;
                            }
                        }
                    }
                    result = !figureOnWay;
                }
                else
                {
                    result = false;
                }
                break;
            #endregion

            case Type.Rook:
                #region Rook
                if (deltaX == 0)
                {
                    for (int j = yMin + 1; j < yMax; j++)
                    {
                        if (figuresOnBoard[finalPosition.x, j] != null)
                        {
                            figureOnWay = true;
                        }
                    }
                    result = !figureOnWay;
                }

                else if (deltaY == 0)
                {
                    for (int i = xMin + 1; i < xMax; i++)
                    {
                        if (figuresOnBoard[i, finalPosition.y] != null)
                        {
                            figureOnWay = true;
                        }
                    }
                    result = !figureOnWay;
                }
                break;
                #endregion
        }
        return result;
    }

    public static bool isNeedToDestroy
        (Figure[,] figuresOnBoard, Figure selectedFigure, Vector2Int finalPosition,
        out Vector2Int destroyPosition)
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

    public static bool isCastling
        (Figure selectedFigure, Vector2Int finalPosition,
        out Vector2Int rookStartPosition, out Vector2Int rookNewPosition)
    {
        int deltaX = Mathf.Abs(finalPosition.x - selectedFigure.figureData.x);
        if (deltaX == 2)
        {
            if (finalPosition.x == 3)
            {
                rookStartPosition = new Vector2Int(0, finalPosition.y);
                rookNewPosition = new Vector2Int(3, finalPosition.y);
                return true;
            }
            else if (finalPosition.x == 6)
            {
                rookStartPosition = new Vector2Int(7, finalPosition.y);
                rookNewPosition = new Vector2Int(5, finalPosition.y);
                return true;
            }
        }
        rookStartPosition = -Vector2Int.one;
        rookNewPosition = -Vector2Int.one;
        return false;
    }

    public static bool isChecked
        (Figure[,] figuresOnBoard, Figure selectedFigure, Vector2Int finalPosition)
    {
        Figure king = null;
        Figure[,] boardCopy = new Figure[8, 8];
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                boardCopy[i, j] = figuresOnBoard[i, j];
            }
        }

        if (Logic.isNeedToDestroy(boardCopy, selectedFigure, finalPosition,
            out Vector2Int destroyPosition))
        {
            boardCopy[destroyPosition.x, destroyPosition.y] = null;
        }

        boardCopy[selectedFigure.figureData.x, selectedFigure.figureData.y] = null;
        boardCopy[finalPosition.x, finalPosition.y] = selectedFigure;
        boardCopy[finalPosition.x, finalPosition.y].figureData.x = finalPosition.x;
        boardCopy[finalPosition.x, finalPosition.y].figureData.y = finalPosition.y;

        foreach (Figure figure in boardCopy)
        {
            if (figure != null)
            {
                if (figure.figureData.type == Type.King
                && figure.figureData.isWhite == selectedFigure.figureData.isWhite)
                {
                    king = figure;
                }
            }
        }

        Vector2Int kingPosition = new Vector2Int(king.figureData.x, king.figureData.y);

        foreach (Figure figure in boardCopy)
        {
            if (figure != null)
            {
                if (figure.figureData.isWhite != selectedFigure.figureData.isWhite)
                {
                    if (isCorrectMove(boardCopy, figure, kingPosition))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public static bool isCheckAndMate(Figure[,] figuresOnBoard, bool isWhite)
    {
        Figure king = null;
        Figure[,] boardCopy = new Figure[8, 8];
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                boardCopy[i, j] = figuresOnBoard[i, j];
            }
        }

        foreach (Figure figure in boardCopy)
        {
            if (figure != null)
            {
                if (figure.figureData.type == Type.King
                && figure.figureData.isWhite == isWhite)
                {
                    king = figure;
                }
            }
        }

        Vector2Int kingPosition = new Vector2Int(king.figureData.x, king.figureData.y);

        foreach (Figure figure in boardCopy)
        {
            if (figure != null)
            {
                if (figure.figureData.isWhite == isWhite)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            Vector2Int position = new Vector2Int(i, j);
                            if (Logic.isCorrectMove(boardCopy, figure, position)
                                && !Logic.isChecked(boardCopy, figure, position))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }
        return false;
    }
}
