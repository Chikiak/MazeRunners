using Core.Interface.Visual;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Core.Controllers;
using Core.Interface.Controllers;
using Core.Interface.Models;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        #region Serialized
        [Header("Maze Configuration")] 
        [SerializeField, Range(3, 7)] private int mazeSize = 10;

        [SerializeField, Range(6, 6)] private int numberOfFaces = 6;
        [SerializeField, Range(1, 100)] private int desarmMoves = 10;
        [SerializeField, Range(1, 4)] private int piecesInTeam = 1;
        [SerializeField, Range(20, 100)] private int totalPoints = 50;
        
        [Header("References")]
        [SerializeField] private AMazeView mazeView;
        [SerializeField] private InputManager inputManager;
        
        //ToDo: [SerializeField] private SOsManager _SOsManager;
        //[SerializeField] private GameObject AuxPanel;
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


        private void SubscribeToActions()
        {
            OnGenerateNewMaze += HandleGenerateNewMaze;
            OnShowFace += HandleShowFace;
            OnRotate += HandleRotate;
            OnDesarmMaze += HandleDesarmMaze;
            OnStateChanged += HandleStateChanged;
        }


        #endregion
        
        #region Properties

        public static GameManager Instance;
        public PlayerID Turn { get; private set; }
        private ICubeController _cubeController;
        
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
            OnStateChanged?.Invoke(GameStates.PieceOnBoardSelection);
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
            if (GameState != GameStates.PieceOnBoardSelection) return;
            _cubeController.Rotate(isRow, clockwise, index);
            //ToDo: Actualizar solo parte cambiada
            mazeView.UpdateMaze(_cubeController.Model.Cells[0]);
        }
        private void HandleDesarmMaze()
        {
            StartCoroutine(DesarmMaze(desarmMoves));
        }
        private void HandleStateChanged(GameStates newState)
        {
            GameState = newState;
        }    
        public static void HandleSelectedCell((int, int) position)
        {
            SelectedCell = position;
            OnSelectedCell?.Invoke();
            if (GameState == GameStates.PutingInitialPiece)
            {
                /*piecesInMaze += 1;
                _cubeController.PutInitialPiece();
                if (piecesInMaze == piecesInTeam * 2)
                {
                    OnStateChanged?.Invoke(GameStates.PieceOnBoardSelection);
                }
                else {OnStateChanged?.Invoke(GameStates.SelectInitialPiece);}
                NewTurn?.Invoke();*/
            }
            else if (ActualAction == ActionType.Move)
            {
                //ToDo
                OnMovePiece?.Invoke(PieceManager.SelectedPiece, SelectedCell);
            }
            OnStateChanged?.Invoke(GameStates.PieceOnBoardSelection);
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
            _cubeController.InitializeMaze(mazeSize);
            PieceManager.Initialize(mazeSize);
            _cubeController.OnCellsChanged += UpdateCells;
        }
        
        private IEnumerator DesarmMaze(int numberOfMoves)
        {
            if (GameManager.GameState != GameStates.PieceOnBoardSelection) yield break;
            for (int i = 0; i < numberOfMoves; i++)
            {
                RandomRotate();

                yield return new WaitForSeconds(0.2f);
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
            
            OnRotate?.Invoke(horizontal, clockwise, r);
        }
        
        #endregion
    }
}