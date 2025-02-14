﻿using Core.Interface.Visual;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Core.Controllers;
using Core.Interface.Controllers;
using Core.Interface.Models;
using TMPro;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        #region Serialized
        [Header("Maze Configuration")] 
        [SerializeField, Range(3, 7)] private int mazeSize = 10;

        [SerializeField, Range(6, 6)] private int numberOfFaces = 6;
        [SerializeField, Range(10, 20)] private int desarmMoves = 10;
        [SerializeField, Range(1, 4)] private int piecesInTeam = 1;
        [SerializeField, Range(20, 100)] private int totalPoints = 50;
        
        [Header("References")]
        [SerializeField] private AMazeView mazeView;
        [SerializeField] private InputManager inputManager;
        [SerializeField] private int TotalRounds;
        [SerializeField] private GameObject Messages;
        [SerializeField] private TMP_Text MessageText;
        private int PointsPlayer1=0;
        private int PointsPlayer2=0;
        public PlayerID Winner;
        
        #endregion
        
        #region Actions

        public static Action OnDesarmMaze;
        public static Action OnGenerateNewMaze;
        public static Action<int> OnShowFace;
        public static Action OnNewTurn;
        public static Action<bool, bool, int > OnRotate;
        public static Action SelectingCell;
        public static Action<GameStates> OnStateChanged;
        public static Action<IPieceController, (int x, int y)> OnMovePiece;
        public static Action OnSelectedCell;
        public static Action<IPieceController> OnPieceSelected;
        public static Action<List<(int x,int y)>> UpdateCellsView;
        public static Action<ActionType> OnChangeActualAction;
        public static Action OnAbilityUsed;
        public static Action OnGameFinished;


        private void SubscribeToActions()
        {
            OnGenerateNewMaze += HandleGenerateNewMaze;
            OnShowFace += HandleShowFace;
            OnRotate += HandleRotate;
            OnDesarmMaze += HandleDesarmMaze;
            OnStateChanged += HandleStateChanged;
            OnPieceSelected += HandlePieceSelected;
            UpdateCellsView += UpdateCells;
            OnNewTurn += HandleNewTurn;
            OnChangeActualAction += HandleChangeAction;
            OnAbilityUsed += HandleAbilityUsed;
            OnGameFinished += GetWinner;
        }

        #endregion
        
        #region Properties

        public static GameManager Instance;
        public static PlayerID Turn { get; private set; }
        private ICubeController _cubeController;
        private static int nOfPiecesInMaze = 0;
        
        public static GameStates GameState { get; private set; }
        public static ActionType ActualAction;
        public static (int x, int y) SelectedCell { get; private set; }
        
        #endregion
        
        #region Unity Methods
        private void Start()
        {
            SubscribeToActions();
            mazeView.InitializeMaze(mazeSize);
            InitializeMaze();
            OnGenerateNewMaze?.Invoke();
            OnShowFace?.Invoke(0);
            OnStateChanged?.Invoke(GameStates.SelectInitialPiece);
            
        }
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        #endregion
        
        #region Handle Actions

        private void HandleGenerateNewMaze()
        {
            _cubeController.GenerateMaze();
        }
        private void HandleShowFace(int faceIndex)
        {
            mazeView.UpdateMaze(_cubeController.Model.Cells[faceIndex]);
        }
        private void HandleRotate(bool isRow, bool clockwise, int index)
        {
            if (GameState != GameStates.PieceOnBoardSelection && GameState != GameStates.Rotate) return;
            _cubeController.Rotate(isRow, clockwise, index);
            //ToDo: Actualizar solo parte cambiada
            mazeView.UpdateMaze(_cubeController.Model.Cells[0]);
            OnNewTurn?.Invoke();
        }
        private void HandleDesarmMaze()
        {
            GameState = GameStates.Rotate;
            StartCoroutine(DesarmMaze(desarmMoves));
        }
        private void HandleStateChanged(GameStates newState)
        {
            GameState = newState;
            Debug.Log($"Game state changed to {GameState}");
        }    
        public static void HandleSelectedCell((int, int) position)
        {
            SelectedCell = position;
            OnSelectedCell?.Invoke();
            if (GameState == GameStates.PutingInitialPiece)
            { 
                nOfPiecesInMaze += 1;
                PieceManager.AddPiece(PieceManager.SelectedPiece, (SelectedCell.x, SelectedCell.y));
                List<(int x, int y)> cells = new List<(int x, int y)>();
                cells.Add((SelectedCell.x, SelectedCell.y));
                UpdateCellsView?.Invoke(cells);
                OnNewTurn?.Invoke();
                if (nOfPiecesInMaze == Instance.piecesInTeam * 2)
                {
                    OnDesarmMaze?.Invoke();
                }
            }
            else if (ActualAction == ActionType.Move)
            {
                OnMovePiece?.Invoke(PieceManager.SelectedPiece, SelectedCell);
                OnNewTurn?.Invoke();
            }
            else if (ActualAction == ActionType.UseAbility)
            {
                OnAbilityUsed?.Invoke();
            }
        }

        public void HandleNewTurn()
        {
            if (TotalRounds == 0) OnGameFinished?.Invoke();
            Turn = Turn switch
            {
                PlayerID.Player2 => PlayerID.Player1,
                _ => PlayerID.Player2
            };
            if (Turn == PlayerID.Player2) TotalRounds--;
            if (GameState == GameStates.PutingInitialPiece)
            {
                OnStateChanged?.Invoke(GameStates.SelectInitialPiece);
            }
            else
            {
                Messages.SetActive(true);
                MessageText.text = $"New Turn: {Turn}";
                PieceManager.HandleDefeatedPieces();
                PieceManager.PiecesNewTurn();
                OnStateChanged?.Invoke(GameStates.PieceOnBoardSelection);
            }
            mazeView.UpdateMaze(_cubeController.Model.Cells[0]);
        }

        public static void HandlePieceSelected(IPieceController piece)
        {
            if (GameState == GameStates.SelectInitialPiece)
            {
                Debug.Log($"{piece} is selected");
                OnStateChanged?.Invoke(GameStates.PutingInitialPiece);
                SelectingCell?.Invoke();
            }
            else if (GameState == GameStates.PieceOnBoardSelection)
            {
                OnStateChanged?.Invoke(GameStates.SelectAction);
            }
        }

        public void HandleChangeAction(ActionType action)
        {
            ActualAction = action;
        }
        public void HandleAbilityUsed()
        {
            ActualAction = ActionType.UseAbility;
        }
        
        
        #endregion
        
        #region Methods


        private void UpdateCells(List<(int x, int y)> positions)
        {
            if (positions == null) return;
            List<ICell> cells = new List<ICell>();
            foreach (var pos in positions)
            {
                cells.Add(_cubeController.Model.Cells[0][pos.x, pos.y]);
            }
            mazeView.UpdateCells(cells);
        }
        private void InitializeMaze()
        {
            _cubeController = new CubeController();//(_SOsManager, maze, generator, mazeView, totalPoints);
            _cubeController.InitializeMaze(mazeSize, totalPoints);
            PieceManager.Initialize(mazeSize);
            _cubeController.OnCellsChanged += UpdateCells;
        }
        
        private IEnumerator DesarmMaze(int numberOfMoves)
        {
            for (int i = 0; i < numberOfMoves; i++)
            {
                RandomRotate();

                yield return new WaitForSeconds(0.2f);
            }
            OnStateChanged?.Invoke(GameStates.PieceOnBoardSelection);
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
            
            OnRotate?.Invoke(horizontal, clockwise, r);
        }

        private void GetWinner()
        {
            for (int i = 0; i < mazeSize; i++)
            {
                for (int j = 0; j < mazeSize; j++)
                {
                    if (PieceManager.PiecesMatrix[i,j].Count < 1) continue;
                    foreach (var piece in PieceManager.PiecesMatrix[i,j])
                    {
                        if (piece.PlayerID == PlayerID.Player1) PointsPlayer1 += piece.PieceModel.Points;
                        else PointsPlayer2 += piece.PieceModel.Points;
                    }
                }
            }
            Winner = PointsPlayer1 > PointsPlayer2 ? PlayerID.Player1 : PlayerID.Player2;
            Messages.SetActive(true);
            MessageText.text = $"Winner: {Winner}";
        }
        
        #endregion
    }
}