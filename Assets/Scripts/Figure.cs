using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Figure : MonoBehaviour
{
    public FigureData figureData;
}

public enum Type
{
    Bishop,
    Knight,
    King,
    Pawn,
    Queen,
    Rook
}

[Serializable]
public class FigureData
{
    public int x;
    public int y;
    public Type type;
    public bool isWhite;

    public FigureData(int x, int y, Type type, bool isWhite)
    {
        this.x = x;
        this.y = y;
        this.type = type;
        this.isWhite = isWhite;
    }
}
