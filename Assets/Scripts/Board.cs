using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class Board : MonoBehaviour
{
    [SerializeField] private GameObject blackBishop;
    [SerializeField] private GameObject blackHorse;
    [SerializeField] private GameObject blackKing;
    [SerializeField] private GameObject blackPawn;
    [SerializeField] private GameObject blackQueen;
    [SerializeField] private GameObject blackRook;
    [SerializeField] private GameObject whiteBishop;
    [SerializeField] private GameObject whiteHorse;
    [SerializeField] private GameObject whiteKing;
    [SerializeField] private GameObject whitePawn;
    [SerializeField] private GameObject whiteQueen;
    [SerializeField] private GameObject whiteRook;
    private FigureData[,] figureDatas;
    private Dictionary<Type, GameObject> WhitePrefabs;
    private Dictionary<Type, GameObject> BlackPrefabs;

    private void Awake()
    {
        WhitePrefabs = new Dictionary<Type, GameObject>()
        {
            {Type.Bishop, whiteBishop},
            {Type.Horse, whiteHorse},
            {Type.King, whiteKing},
            {Type.Pawn, whitePawn},
            {Type.Queen, whiteQueen},
            {Type.Rook, whiteRook}
        };
        BlackPrefabs = new Dictionary<Type, GameObject>()
        {
            {Type.Bishop, blackBishop},
            {Type.Horse, blackHorse},
            {Type.King, blackKing},
            {Type.Pawn, blackPawn},
            {Type.Queen, blackQueen},
            {Type.Rook, blackRook}
        };
    }

    private void Start()
    {
        figureDatas = new FigureData[8, 8];
        AddFigure(new FigureData(0, 0, Type.Bishop, true));
        AddFigure(new FigureData(1, 0, Type.Horse, true));
        AddFigure(new FigureData(2, 0, Type.Rook, true));
        AddFigure(new FigureData(3, 0, Type.King, true));
        AddFigure(new FigureData(4, 0, Type.Queen, true));
        AddFigure(new FigureData(5, 0, Type.Rook, true));
        AddFigure(new FigureData(6, 0, Type.Horse, true));
        AddFigure(new FigureData(7, 0, Type.Bishop, true));
        AddFigure(new FigureData(0, 1, Type.Pawn, true));
        AddFigure(new FigureData(1, 1, Type.Pawn, true));
        AddFigure(new FigureData(2, 1, Type.Pawn, true));
        AddFigure(new FigureData(3, 1, Type.Pawn, true));
        AddFigure(new FigureData(4, 1, Type.Pawn, true));
        AddFigure(new FigureData(5, 1, Type.Pawn, true));
        AddFigure(new FigureData(6, 1, Type.Pawn, true));
        AddFigure(new FigureData(7, 1, Type.Pawn, true));

        AddFigure(new FigureData(0, 7, Type.Bishop, false));
        AddFigure(new FigureData(1, 7, Type.Horse, false));
        AddFigure(new FigureData(2, 7, Type.Rook, false));
        AddFigure(new FigureData(3, 7, Type.King, false));
        AddFigure(new FigureData(4, 7, Type.Queen, false));
        AddFigure(new FigureData(5, 7, Type.Rook, false));
        AddFigure(new FigureData(6, 7, Type.Horse, false));
        AddFigure(new FigureData(7, 7, Type.Bishop, false));
        AddFigure(new FigureData(0, 6, Type.Pawn, false));
        AddFigure(new FigureData(1, 6, Type.Pawn, false));
        AddFigure(new FigureData(2, 6, Type.Pawn, false));
        AddFigure(new FigureData(3, 6, Type.Pawn, false));
        AddFigure(new FigureData(4, 6, Type.Pawn, false));
        AddFigure(new FigureData(5, 6, Type.Pawn, false));
        AddFigure(new FigureData(6, 6, Type.Pawn, false));
        AddFigure(new FigureData(7, 6, Type.Pawn, false));
        SaveBoard();
    }

    private void Update()
    {

    }

    public void SaveBoard()
    {
        string json = SerializeBoard(figureDatas);
        SaveToJSON(json, "newGame.json");
    }

    public void LoadBoard(string path)
    {

    }

    public void SaveToJSON(string json, string path)
    {
        string fullPath = Path.Combine(Application.streamingAssetsPath, path);
        using (StreamWriter streamWriter = new StreamWriter(fullPath))
        {
            streamWriter.Write(json);
        }
    }

    public List<FigureData> LoadFromJSON(string path)
    {
        return null;
    }

    public string SerializeBoard(FigureData[,] figureDatas)
    {
        List<FigureData> fdlist = new List<FigureData>();
        foreach (var item in figureDatas)
        {
            if (item != null)
            {
                fdlist.Add(item);
            }
        }
        BoardState boardState;
        boardState.figureDatas = fdlist;
        string json = JsonUtility.ToJson(boardState);
        return json;
    }

    public void GenerateBoard(List<FigureData> figureDatas)
    {

    }

    public void CleanBoard(List<FigureData> figureDatas)
    {

    }

    public void AddFigure(FigureData figureData)
    {
        GameObject figurePrefab;
        Vector3 figurePos = new Vector3(figureData.x, 0, figureData.y);
        if (figureData.isWhite)
        {
            figurePrefab = WhitePrefabs[figureData.type];
        }
        else
        {
            figurePrefab = BlackPrefabs[figureData.type];
        }
        var figure = Instantiate(figurePrefab, figurePos, transform.rotation, transform.parent);
        figureDatas[figureData.x, figureData.y] = figureData;
    }
}

[Serializable]
public struct BoardState
{
    public List<FigureData> figureDatas;
    //public bool isWhiteTurn;
}