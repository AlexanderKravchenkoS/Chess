using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class Board : MonoBehaviour
{
    [SerializeField] private GameObject blackBishop;
    [SerializeField] private GameObject blackKnight;
    [SerializeField] private GameObject blackKing;
    [SerializeField] private GameObject blackPawn;
    [SerializeField] private GameObject blackQueen;
    [SerializeField] private GameObject blackRook;
    [SerializeField] private GameObject whiteBishop;
    [SerializeField] private GameObject whiteKnight;
    [SerializeField] private GameObject whiteKing;
    [SerializeField] private GameObject whitePawn;
    [SerializeField] private GameObject whiteQueen;
    [SerializeField] private GameObject whiteRook;
    private Dictionary<Type, GameObject> WhitePrefabs;
    private Dictionary<Type, GameObject> BlackPrefabs;
    private BoardState boardState;
    public Figure[,] figures;
    public List<FigureData> figureDatas;

    private void Awake()
    {
        WhitePrefabs = new Dictionary<Type, GameObject>()
        {
            {Type.Bishop, whiteBishop},
            {Type.Knight, whiteKnight},
            {Type.King, whiteKing},
            {Type.Pawn, whitePawn},
            {Type.Queen, whiteQueen},
            {Type.Rook, whiteRook}
        };
        BlackPrefabs = new Dictionary<Type, GameObject>()
        {
            {Type.Bishop, blackBishop},
            {Type.Knight, blackKnight},
            {Type.King, blackKing},
            {Type.Pawn, blackPawn},
            {Type.Queen, blackQueen},
            {Type.Rook, blackRook}
        };
    }

    private void Start()
    {

    }

    private void Update()
    {

    }

    public void LoadGame(string path)
    {
        LoadBoard(path, ref boardState);
        Figure[] ff = FindObjectsOfType<Figure>();
        figures = new Figure[8, 8];
        foreach (var item in ff)
        {
            figures[item.figureData.x, item.figureData.y] = item;
        }
    }

    public void SaveGame()
    {
        boardState.figureDatas = new List<FigureData>();
        foreach (var item in figures)
        {
            if (item != null)
            {
                boardState.figureDatas.Add(item.figureData);
            }
        }
        SaveBoard("previousGame.json", boardState);
    }

    private void SaveBoard(string path, BoardState boardState)
    {
        string json = SerializeBoard(boardState);
        SaveToJSON(json, path);
    }

    private void LoadBoard(string path, ref BoardState boardState)
    {
        CleanBoard();
        boardState = LoadFromJSON(path);
        GenerateBoard(boardState);
    }

    private void SaveToJSON(string json, string path)
    {
        string fullPath = Path.Combine(Application.streamingAssetsPath, path);
        using (StreamWriter streamWriter = new StreamWriter(fullPath))
        {
            streamWriter.Write(json);
        }
    }

    private BoardState LoadFromJSON(string path)
    {
        string fullPath = Path.Combine(Application.streamingAssetsPath, path);
        using StreamReader reader = new StreamReader(fullPath);
        string json = reader.ReadToEnd();
        BoardState boardState = JsonUtility.FromJson<BoardState>(json);
        return boardState;
    }

    private string SerializeBoard(BoardState boardState)
    {
        string json = JsonUtility.ToJson(boardState);
        return json;
    }

    private void GenerateBoard(BoardState boardState)
    {
        foreach (var item in boardState.figureDatas)
        {
            AddFigure(item);
        }
    }

    private void CleanBoard()
    {
        Figure[] figures = FindObjectsOfType<Figure>();
        foreach (var item in figures)
        {
            if (item != null)
            {
                Destroy(item.gameObject);
            }
        }
    }

    private void AddFigure(FigureData figureData)
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
        var figure = Instantiate(figurePrefab, figurePos, transform.rotation, transform);
        figure.GetComponent<Figure>().figureData = figureData;
        figureDatas.Add(figureData);
    }
}

[Serializable]
public struct BoardState
{
    public List<FigureData> figureDatas;
    public bool isWhiteTurn;
}