using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using TMPro;

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

    [SerializeField] private GameObject MainMenuCanvas;
    [SerializeField] private GameObject PauseCanvas;
    [SerializeField] private GameObject SelectCanvas;
    [SerializeField] private GameObject ErrorCanvas;

    [SerializeField] private GameObject Highlighter;

    private Dictionary<Type, GameObject> WhitePrefabs;
    private Dictionary<Type, GameObject> BlackPrefabs;

    private BoardState boardState;

    private Figure selectedFigure;
    private Figure pawnToDestroy;
    private FigureData lastFigure = null;

    private GameState gameState = GameState.Running;

    private string newGamePath;
    private string previousGamePath;
    private const float highlihterY = 0.01f;
    private const float selectedFigureY = 2f;

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

        newGamePath = Application.persistentDataPath + "/newGame.json";
        previousGamePath = Application.persistentDataPath + "/previousGame.json";

        if (!File.Exists(newGamePath))
        {
            using StreamReader reader = new StreamReader
                (Path.Combine(Application.streamingAssetsPath, "newGame.json"));
            string json = reader.ReadToEnd();
            File.WriteAllText(newGamePath, json);
        }
    }

    private void Start()
    {
        MainMenuCanvas.SetActive(true);
        PauseCanvas.SetActive(false);
        ErrorCanvas.SetActive(false);
        SelectCanvas.SetActive(false);
        gameState = GameState.Paused;
    }

    private void Update()
    {
        if (gameState == GameState.Running)
        {
            MakeTurn();
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            gameState = GameState.Paused;
            PauseCanvas.SetActive(true);
        }
    }

    private void MakeTurn()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, LayerMask.GetMask("Board")))
        {
            int mouseX = (int)(hit.point.x + 0.5);
            int mouseY = (int)(hit.point.z + 0.5);

            Highlighter.SetActive(true);
            Highlighter.transform.position = new Vector3(mouseX, highlihterY, mouseY);

            if (Input.GetMouseButtonDown(0))
            {
                var figure = hit.transform.gameObject.GetComponent<Figure>();
                if (figure != null && figure.figureData.isWhite == boardState.isWhiteTurn)
                {
                    selectedFigure = figure;
                }
            }

            if (selectedFigure != null)
            {
                selectedFigure.transform.position = new Vector3(mouseX, selectedFigureY, mouseY);

                if (Input.GetMouseButtonUp(0))
                {
                    Figure[,] figures = new Figure[8, 8];
                    Figure[] figuresOnBoard = FindObjectsOfType<Figure>();
                    foreach (var item in figuresOnBoard)
                    {
                        if (item != null)
                        {
                            figures[item.figureData.x, item.figureData.y] = item;
                        }
                    }

                    if (Logic.isCorrectMove(figures, selectedFigure, lastFigure, mouseX, mouseY)
                        && !Logic.isChecked(figures, selectedFigure, mouseX, mouseY))
                    {
                        if (Logic.isCastling(selectedFigure,mouseX, out int oldX, out int newX))
                        {
                            figures[oldX, mouseY].gameObject.transform.position =
                                new Vector3(newX, 0, mouseY);
                            figures[oldX, mouseY].figureData.x = newX;
                            figures[oldX, mouseY].figureData.turnCount++;
                            figures[newX, mouseY] = figures[oldX, mouseY];
                            figures[oldX, mouseY] = null;
                        }
                        else if (Logic.isNeedToDestroy(figures, selectedFigure, mouseX, mouseY,
                            out int destroyX, out int destoryY))
                        {
                            Destroy(figures[destroyX, destoryY].gameObject);
                            figures[destroyX, destoryY] = null;
                        }

                        figures[selectedFigure.figureData.x, selectedFigure.figureData.y] = null;
                        selectedFigure.figureData.x = mouseX;
                        selectedFigure.figureData.y = mouseY;
                        selectedFigure.figureData.turnCount++;
                        figures[mouseX, mouseY] = selectedFigure;

                        lastFigure = selectedFigure.figureData;
                        boardState.isWhiteTurn = !boardState.isWhiteTurn;

                        if (Logic.isPawnInTheEnd(lastFigure))
                        {
                            gameState = GameState.Stopped;
                            SelectCanvas.SetActive(true);
                            pawnToDestroy = selectedFigure;
                        }

                        var newGameState =
                            Logic.isMateOrDraw(figures, boardState.isWhiteTurn, lastFigure);

                        if (newGameState == GameState.Draw)
                        {
                            gameState = GameState.Draw;
                            MainMenuCanvas.GetComponentInChildren<TextMeshProUGUI>().text = "Draw";
                            MainMenuCanvas.SetActive(true);
                        }
                        else if (newGameState == GameState.Mate)
                        {
                            gameState = GameState.Mate;
                            if (boardState.isWhiteTurn)
                            {
                                MainMenuCanvas.GetComponentInChildren<TextMeshProUGUI>().text = "Black Win";
                                MainMenuCanvas.SetActive(true);
                            }
                            else
                            {
                                MainMenuCanvas.GetComponentInChildren<TextMeshProUGUI>().text = "White Win";
                                MainMenuCanvas.SetActive(true);
                            }
                        }
                    }

                    selectedFigure.transform.position = new Vector3
                        (selectedFigure.figureData.x, 0, selectedFigure.figureData.y);
                    selectedFigure = null;
                }
            }
        }
        else
        {
            Highlighter.SetActive(false);
        }
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
        using (StreamWriter streamWriter = new StreamWriter(path))
        {
            streamWriter.Write(json);
        }
    }

    private BoardState LoadFromJSON(string path)
    {
        BoardState boardState = new BoardState();
        if (File.Exists(path))
        {
            using StreamReader reader = new StreamReader(path);
            string json = reader.ReadToEnd();
            boardState = JsonUtility.FromJson<BoardState>(json);
        }
        return boardState;
    }

    private string SerializeBoard(BoardState boardState)
    {
        string json = JsonUtility.ToJson(boardState);
        return json;
    }

    private void GenerateBoard(BoardState boardState)
    {
        if (boardState.figureDatas != null)
        {
            foreach (var item in boardState.figureDatas)
            {
                AddFigure(item);
            }
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
        var figure =
            Instantiate(figurePrefab, figurePos, figurePrefab.transform.rotation, transform);
        figure.GetComponent<Figure>().figureData = figureData;
    }

    public void SaveGame()
    {
        Figure[] figuresOnBoard = FindObjectsOfType<Figure>();
        boardState.lastFigure = lastFigure;
        boardState.figureDatas = new List<FigureData>();
        foreach (var item in figuresOnBoard)
        {
            if (item != null)
            {
                boardState.figureDatas.Add(item.figureData);
            }
        }
        SaveBoard(previousGamePath, boardState);
    }

    public void LoadPreviousGame()
    {
        if (!File.Exists(previousGamePath)) {
            ErrorCanvas.SetActive(true);
        } else {
            LoadBoard(previousGamePath, ref boardState);
            lastFigure = boardState.lastFigure;
            gameState = GameState.Running;
            MainMenuCanvas.SetActive(false);
            PauseCanvas.SetActive(false);
        }
    }

    public void LoadNewGame()
    {
        LoadBoard(newGamePath, ref boardState);
        lastFigure = boardState.lastFigure;
        gameState = GameState.Running;
        MainMenuCanvas.SetActive(false);
        PauseCanvas.SetActive(false);
    }

    public void CloseError() {
        ErrorCanvas.SetActive(false);
    }

    public void Exit() {
        Application.Quit();
    }

    public void CreateQueen()
    {
        DestroyPawnAndCreateFigure(Type.Queen);
    }

    public void CreateKnight()
    {
        DestroyPawnAndCreateFigure(Type.Knight);
    }

    public void CreateBishop()
    {
        DestroyPawnAndCreateFigure(Type.Bishop);
    }

    public void CreateRook()
    {
        DestroyPawnAndCreateFigure(Type.Rook);
    }

    private void DestroyPawnAndCreateFigure(Type type)
    {
        Destroy(pawnToDestroy.gameObject);
        pawnToDestroy = null;
        lastFigure.type = type;
        AddFigure(lastFigure);
        gameState = GameState.Running;
        SelectCanvas.SetActive(false);
    }
}

[Serializable]
public struct BoardState
{
    public List<FigureData> figureDatas;
    public FigureData lastFigure;
    public bool isWhiteTurn;
}

public enum GameState
{
    Paused,
    Running,
    Stopped,
    Mate,
    Draw
}