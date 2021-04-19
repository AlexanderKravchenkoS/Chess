using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Logic
{
    private const int minBound = 0;
    private const int maxBound = 7;
    public static bool isCorrectMove
        (Figure[,] figures, Figure selectedFigure, FigureData lastFigure, int newX, int newY)
    {
        if (newX < minBound || newX > maxBound || newY < minBound || newY > maxBound)
        {
            return false;
        }

        if (newX == selectedFigure.figureData.x && newY == selectedFigure.figureData.y)
        {
            return false;
        }

        if (figures[newX, newY] != null)
        {
            if (figures[newX, newY].figureData.isWhite == selectedFigure.figureData.isWhite)
            {
                return false;
            }
        }

        bool result = false;
        bool figureOnWay = false;
        int deltaX = newX - selectedFigure.figureData.x;
        int deltaY = newY - selectedFigure.figureData.y;
        int deltaXabs = Mathf.Abs(deltaX);
        int deltaYabs = Mathf.Abs(deltaY);

        switch (selectedFigure.figureData.type)
        {
            case Type.Bishop:
                #region Bishop
                if (deltaXabs == deltaYabs)
                {
                    int stepY = deltaY / deltaYabs;
                    int stepX = deltaX / deltaXabs;
                    for (int i = selectedFigure.figureData.x + stepX; i != newX; i += stepX)
                    {
                        for (int j = selectedFigure.figureData.y + stepY; j != newY; j += stepY)
                        {
                            if ((i - selectedFigure.figureData.x) * stepX ==
                                (j - selectedFigure.figureData.y) * stepY
                                && figures[i,j] != null && figureOnWay == false)
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
                if ((deltaXabs == 0 || deltaXabs == 1) && (deltaYabs == 0 || deltaYabs == 1))
                {
                    result = true;
                }
                else if (selectedFigure.figureData.turnCount == 0 && deltaYabs == 0
                    && deltaXabs == 2)
                {
                    if (deltaX == 2 && figures[maxBound, newY] != null)
                    {
                        if (figures[maxBound, newY].figureData.type == Type.Rook
                            && figures[maxBound, newY].figureData.turnCount == 0)
                        {
                            result = true;
                        }
                    }
                    else if (deltaX == -2 && figures[minBound, newY] != null)
                    {
                        if (figures[minBound, newY].figureData.type == Type.Rook
                            && figures[minBound, newY].figureData.turnCount == 0)
                        {
                            result = true;
                        }
                    }
                }
                break;
            #endregion

            case Type.Knight:
                #region Knight
                if ((deltaXabs == 1 && deltaYabs == 2) || (deltaXabs == 2 && deltaYabs == 1))
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

                if (deltaX == 0 && deltaY == direction && figures[newX, newY] == null)
                {
                    result = true;
                }
                else if (deltaX == 0 && deltaY == (direction * 2) && figures[newX, newY] == null
                    && figures[newX, newY - direction] == null
                    && selectedFigure.figureData.turnCount == 0)
                {
                    result = true;
                }
                else if (deltaXabs == 1 && deltaY == direction && figures[newX, newY] != null)
                {
                    result = true;
                }
                else if (deltaXabs == 1 && deltaY == direction && figures[newX, newY] == null
                    && figures[newX, selectedFigure.figureData.y] != null)
                {
                    if (newX == lastFigure.x && selectedFigure.figureData.y == lastFigure.y
                        && lastFigure.turnCount == 1)
                    {
                        result = true;
                    }
                }
                break;
            #endregion

            case Type.Queen:
                #region Queen
                if (deltaXabs == deltaYabs)
                {
                    int stepY = deltaY / deltaYabs;
                    int stepX = deltaX / deltaXabs;
                    for (int i = selectedFigure.figureData.x + stepX; i != newX; i += stepX)
                    {
                        for (int j = selectedFigure.figureData.y + stepY; j != newY; j += stepY)
                        {
                            if ((i - selectedFigure.figureData.x) * stepX ==
                                (j - selectedFigure.figureData.y) * stepY
                                && figures[i, j] != null && figureOnWay == false)
                            {
                                figureOnWay = true;
                            }
                        }
                    }
                    result = !figureOnWay;
                }

                else if (deltaX == 0)
                {
                    int stepY = deltaY / deltaYabs;
                    for (int j = selectedFigure.figureData.y + stepY; j != newY; j += stepY)
                    {
                        if (figureOnWay == false && figures[newX, j] != null)
                        {
                            figureOnWay = true;
                        }
                    }
                    result = !figureOnWay;
                }

                else if (deltaY == 0)
                {
                    int stepX = deltaX / deltaXabs;
                    for (int i = selectedFigure.figureData.x + stepX; i != newX; i += stepX)
                    {
                        if (figureOnWay == false && figures[i, newY] != null)
                        {
                            figureOnWay = true;
                        }
                    }
                    result = !figureOnWay;
                }
                break;
            #endregion

            case Type.Rook:
                #region Rook
                if (deltaX == 0)
                {
                    int stepY = deltaY / deltaYabs;
                    for (int j = selectedFigure.figureData.y + stepY; j != newY; j += stepY)
                    {
                        if (figureOnWay == false && figures[newX, j] != null)
                        {
                            figureOnWay = true;
                        }
                    }
                    result = !figureOnWay;
                }
                else if (deltaY == 0)
                {
                    int stepX = deltaX / deltaXabs;
                    for (int i = selectedFigure.figureData.x + stepX; i != newX; i += stepX)
                    {
                        if (figureOnWay == false && figures[i, newY] != null)
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

    public static bool isNeedToDestroy(Figure[,] figures, Figure selectedFigure,
        int newX, int newY, out int destroyX, out int destroyY)
    {
        if (figures[newX, newY] != null)
        {
            destroyX = newX;
            destroyY = newY;
            return true;
        }

        if (selectedFigure.figureData.type == Type.Pawn
            && Mathf.Abs(newX - selectedFigure.figureData.x) == 1)
        {
            destroyX = newX;
            destroyY = selectedFigure.figureData.y;
            return true;
        }

        destroyX = destroyY = 0;
        return false;
    }

    public static bool isCastling(Figure selectedFigure, int newX,
        out int rookOldX, out int rookNewX)
    {
        if (selectedFigure.figureData.type == Type.King)
        {
            int deltaX = Mathf.Abs(newX - selectedFigure.figureData.x);
            if (deltaX == 2)
            {
                if (newX == 2)
                {
                    rookOldX = 0;
                    rookNewX = 3;
                    return true;
                }
                else if (newX == 6)
                {
                    rookOldX = 7;
                    rookNewX = 5;
                    return true;
                }
            }
        }
        rookNewX = rookOldX = 0;
        return false;
    }

    public static bool isChecked
        (Figure[,] figures, Figure selectedFigure, int newX, int newY)
    {
        int originalX = selectedFigure.figureData.x;
        int originalY = selectedFigure.figureData.y;
        int turnCount = selectedFigure.figureData.turnCount;
        Figure king = null;
        Figure[,] boardCopy = new Figure[8,8];

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (figures[i,j] != null)
                {
                    boardCopy[i, j] = figures[i, j];
                }
            }
        }

        if (Logic.isCastling(selectedFigure, newX, out int oldRookX, out int newRookX))
        {
            boardCopy[oldRookX, newY].gameObject.transform.position =
                new Vector3(newRookX, 0, newY);
            boardCopy[oldRookX, newY].figureData.x = newRookX;
            boardCopy[oldRookX, newY].figureData.turnCount++;
            boardCopy[newRookX, newY] = figures[oldRookX, newY];
            boardCopy[oldRookX, newY] = null;
        }
        else if (Logic.isNeedToDestroy(figures, selectedFigure, newX, newY,
            out int destroyX, out int destoryY))
        {
            boardCopy[destroyX, destoryY] = null;
        }

        boardCopy[selectedFigure.figureData.x, selectedFigure.figureData.y] = null;
        selectedFigure.figureData.x = newX;
        selectedFigure.figureData.y = newY;
        selectedFigure.figureData.turnCount++;
        boardCopy[newX, newY] = selectedFigure;

        FigureData lastFigure = selectedFigure.figureData;

        foreach (var figure in boardCopy)
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

        int kingX = king.figureData.x;
        int kingY = king.figureData.y;

        foreach (var figure in boardCopy)
        {
            if (figure != null && figure.figureData.isWhite != king.figureData.isWhite)
            {
                if (isCorrectMove(boardCopy, figure, lastFigure, kingX, kingY))
                {
                    selectedFigure.figureData.x = originalX;
                    selectedFigure.figureData.y = originalY;
                    return true;
                }
            }
        }
        selectedFigure.figureData.x = originalX;
        selectedFigure.figureData.y = originalY;
        selectedFigure.figureData.turnCount = turnCount;
        return false;
    }

    public static bool isCheckAndMate(Figure[,] figures, bool isWhite, FigureData lastFigure)
    {
        Figure king = null;
        Figure[,] boardCopy = new Figure[8, 8];

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (figures[i, j] != null)
                {
                    boardCopy[i, j] = figures[i, j];
                }
            }
        }

        foreach (var figure in boardCopy)
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

        int kingX = king.figureData.x;
        int kingY = king.figureData.y;

        foreach (var item in boardCopy)
        {
            if (item != null && item.figureData.isWhite == isWhite)
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (isCorrectMove(boardCopy, item, lastFigure, i, j)
                            && !isChecked(boardCopy, item, i, j))
                        {
                            return false;
                        }
                    }
                }
            }
        }
        return true;
    }

    public static bool isPawnInTheEnd(FigureData selectedFigure)
    {
        if (selectedFigure.type == Type.Pawn && (selectedFigure.y == minBound
            || selectedFigure.y == maxBound))
        {
            return true;
        }
        return false;
    }
}
