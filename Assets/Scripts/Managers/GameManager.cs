using Core.Interface.Visual;
using UnityEngine;
using System;
using Core.Controllers;
using Core.Interface.Controllers;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        #region Serialized
        [Header("Maze Configuration")] 
        [SerializeField, Range(3, 20)] private int mazeSize = 10;

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

        private void SubscribeToActions()
        {
            OnGenerateNewMaze += HandleGenerateNewMaze;
            OnShowFace += HandleShowFace;
            OnRotate += HandleRotate;
        }

        

        #endregion
        
        #region Properties

        public PlayerID Turn { get; private set; }
        private ICubeController _cubeController;
        
        #endregion
        
        #region Unity Methods
        private void Start()
        {
            mazeView.InitializeMaze(mazeSize);
            SubscribeToActions();
            InitializeMaze();
            OnGenerateNewMaze?.Invoke();
            OnShowFace?.Invoke(0);
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
            _cubeController.Rotate(isRow, clockwise, index);
            //ToDo: Actualizar solo parte cambiada
            mazeView.UpdateMaze(_cubeController.Model.Cells[0]);
        }
        
        
        #endregion
        
        #region Methods
        
        private void InitializeMaze()
        {
            _cubeController = new CubeController();//(_SOsManager, maze, generator, mazeView, totalPoints);
            _cubeController.InitializeMaze(mazeSize);
            //ToDo: _cubeController.OnCellSelected += HandleSelectedCell;
        }
        #endregion
    }
}