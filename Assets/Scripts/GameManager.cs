using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controllers;
using Core.Models;
using Core.Generators;
using Core.Interfaces;
using Core.Interfaces.Entities;
using Views;
using Views.Selection;
using MazeView = Views.MazeView;

public class GameManager : MonoBehaviour
{
    #region Properties
    public static GameManager Instance { get; private set; }
    public static ITokenController SelectedToken { get; private set; }
    public static ITokenController SecondToken { get; private set; }
    public static (int,int) SelectedCell { get; private set; }

    [Header("Maze Configuration")]
    [SerializeField, Range(5, 20)] private int mazeSize = 10;
    [SerializeField, Range(6, 6)] private int numberOfFaces = 1;
    [SerializeField, Range(1, 100)] private int desarmMoves = 20;
    [SerializeField, Range(1, 4)] private int piecesInTeam = 1;
    private int piecesInMaze = 0;
    
    [Header("References")]
    [SerializeField] private MazeView mazeView;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private SOsManager _SOsManager;
    [SerializeField] private GameObject AuxPanel;

    private MazeController _mazeController;
    public static Action OnMazeChanged;
    public static Action SelectingCell;
    public static Action PieceSelected;
    public static Action NewTurn;
    public static Action<GameStates> OnStateChanged;
    public static Action<ITokenController> AbilityUsed;
    public static Action<IToken> OnShowInfo;
    public static Action<ITokenController, (int,int)> MovePiece;
    public static Action<(int,int), TrapTypes> ActivateTrap;
    public static Action<ITokenController> DeadPiece;
    public static GameStates GameState {get; private set;}
    public static ActionTypes ActualAction {get; private set;}
    public static Players Turn {get; private set;}
    public static bool MazeChanged {get; private set;}
    public static int RoundNumber = 0;
    public static List<ITokenController> DeadTokens = new List<ITokenController>();
    #endregion

    #region DontTouch
    private void Awake()
    {
        ValidateInstance();
    }

    private void OnEnable()
    {
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    private void Start()
    {
        TokenFactory.sosManager = _SOsManager;
    }

    private void ValidateInstance()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    #endregion

    #region Events
    private void SubscribeToEvents()
    {
        InputManager.OnGenerateNewMaze += GenerateNewMaze;
        InputManager.OnDesarmMaze += HandleDesarmMaze;
        MazeRotationArrows.OnRowRotate += HandleRowRotate;
        MazeRotationArrows.OnColumnRotate += HandleColumnRotate;
        SOsManager.Ready += HandleStateChanged;
        var aux = AuxPanel.GetComponent<AuxPanel>();
        aux.OnTokenSelected += InitialSelectToken;
        aux.OnTokenInfo += ShowInfo;
        NewTurn += HandleNewTurn;
        PieceSelected += HandlePieceSelected;
        OnStateChanged += HandleStateChanged;
    }
    private void UnsubscribeFromEvents()
    {
        InputManager.OnGenerateNewMaze -= GenerateNewMaze;
        InputManager.OnDesarmMaze -= HandleDesarmMaze;
        MazeRotationArrows.OnRowRotate -= HandleRowRotate;
        MazeRotationArrows.OnColumnRotate -= HandleColumnRotate;
        SOsManager.Ready -= HandleStateChanged;
        var aux = AuxPanel.GetComponent<AuxPanel>();
        aux.OnTokenSelected -= InitialSelectToken;
        aux.OnTokenInfo -= ShowInfo;
        _mazeController.OnCellSelected -= HandleSelectedCell;
        NewTurn -= HandleNewTurn;
        PieceSelected -= HandlePieceSelected;
        OnStateChanged -= HandleStateChanged;
    }
    private void HandleRowRotate(int rowIndex, bool clockwise)
    {
        MazeChanged = true;
        _mazeController.RotateFace(true, rowIndex, clockwise);
        OnMazeChanged?.Invoke();
        NewTurn?.Invoke();
    }
    private void HandleColumnRotate(int columnIndex, bool clockwise)
    {
        MazeChanged = true;
        _mazeController.RotateFace(false, columnIndex, clockwise);
        OnMazeChanged?.Invoke();
        NewTurn?.Invoke();
    }
    private void HandleDesarmMaze()
    {
        StartCoroutine(DesarmMaze(desarmMoves));
    }
    private void HandleStateChanged(GameStates newState)
    {
        switch (newState)
        {
            case GameStates.Starting:
                StartGame();
                break;
            case GameStates.SelectInitialPiece :
                GameState = GameStates.SelectInitialPiece;
                break;
            case GameStates.PutingInitialPiece:
                GameState = GameStates.PutingInitialPiece;
                break;
            case GameStates.PieceOnBoardSelection:
                GameState = GameStates.PieceOnBoardSelection;
                break;
            case GameStates.CellSelection:
                GameState = GameStates.CellSelection;
                break;
            case GameStates.SelectAction:
                GameState = GameStates.SelectAction;
                break;
        }
        Debug.Log($"Game state changed to {GameState}");
    }
    public void InitialSelectToken(TokensNames newToken)
    {
        var TokenC = TokenFactory.NewToken(newToken);
        SelectedToken = TokenC;
        Debug.Log($"Selected token {newToken}");
        OnStateChanged?.Invoke(GameStates.PutingInitialPiece);
        SelectingCell?.Invoke();
    }
    private void HandleSelectedCell((int, int) position)
    {
        SelectedCell = position;
        if (GameState == GameStates.PutingInitialPiece)
        {
            piecesInMaze += 1;
            _mazeController.PutInitialPiece();
            if (piecesInMaze == piecesInTeam * 2)
            {
                OnStateChanged?.Invoke(GameStates.PieceOnBoardSelection);
            }
            else {OnStateChanged?.Invoke(GameStates.SelectInitialPiece);}
            NewTurn?.Invoke();
        }
        else if (ActualAction == ActionTypes.Move)
        {
            MovePiece?.Invoke(SelectedToken, SelectedCell);
        }
    }
    private void ShowInfo(TokensNames tokenM)
    {
        IToken token;
        if (GameState != GameStates.PutingInitialPiece
            && GameState != GameStates.SelectInitialPiece)
        {
            token = _mazeController.GetToken(tokenM);
        }
        token = _SOsManager.GetToken(tokenM);
        OnShowInfo?.Invoke(token);
        Debug.Log($"Info of: {token.TokenName}");
    }
    private void HandleNewTurn()
    {
        switch (Turn)
        {
            case Players.Player1:
                Turn = Players.Player2;
                break;
            case Players.Player2:
                Turn = Players.Player1;
                break;
        }
        if (Turn == Players.Player1) RoundNumber++;
        Debug.Log($"New Turn:{Turn}, Round: {RoundNumber}");
        MazeChanged = false;
        if (GameState != GameStates.PutingInitialPiece && GameState != GameStates.SelectInitialPiece)
        {
            OnStateChanged?.Invoke(GameStates.PieceOnBoardSelection);
        }

        ActualAction = ActionTypes.StartTurn;
    }
    private void HandlePieceSelected()
    {
        OnStateChanged?.Invoke(GameStates.SelectAction);
    }
    #endregion

    public static void ChangeActualAction(ActionTypes actionType)
    {
        ActualAction = actionType;
    }
    public static void SelectToken(ITokenController token)
    {
        if (ActualAction == ActionTypes.StartTurn)
        {
            SelectedToken = token;
        }
        else if (ActualAction == ActionTypes.UseAbility)
        {
            SecondToken = token;
        }
    }
    private void StartGame()
    {
        GenerateNewMaze();
        AuxPanel.SetActive(true);
        HandleStateChanged(GameStates.SelectInitialPiece);
    }
    private IEnumerator DesarmMaze(int numberOfMoves)
    {
        for (int i = 0; i < numberOfMoves; i++)
        {
            RandomRotate();
            OnMazeChanged?.Invoke();
        
            yield return new WaitForSeconds(1f);
        }
    }
    private void RandomRotate()
    {
        int r = (int) UnityEngine.Random.Range(0, mazeSize);
        int c = (int) UnityEngine.Random.Range(0, 10);
        int f = (int) UnityEngine.Random.Range(0, 10);
        bool horizontal = (c % 2 == 0);
        bool clockwise = (f % 2 == 0);
        if (horizontal)
        {
            Debug.Log(clockwise ? "Derecha" : "Izquierda");
        }
        else
        {
            Debug.Log(clockwise ? "Abajo" : "Arriba");
        }
        
        _mazeController.RotateFace(horizontal, r, clockwise);
    }
    private void InitializeMaze()
    {
        var maze = new Maze(mazeSize, numberOfFaces, _SOsManager);
        var generator = new MazeGenerator(mazeSize, 10);
        
        _mazeController = new MazeController(_SOsManager, maze, generator, mazeView);
        _mazeController.OnCellSelected += HandleSelectedCell;
    }
    public void GenerateNewMaze()
    {
        OnMazeChanged = null;
        InitializeMaze();
        _mazeController.GenerateMaze();
        OnMazeChanged?.Invoke();
    }

}