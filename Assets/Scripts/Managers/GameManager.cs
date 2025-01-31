using Core.Interface.Visual;
using UnityEngine;
using System;
using System.Collections;
using Core.Controllers;
using Core.Interface.Controllers;

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

        private void SubscribeToActions()
        {
            OnGenerateNewMaze += HandleGenerateNewMaze;
            OnShowFace += HandleShowFace;
            OnRotate += HandleRotate;
            OnDesarmMaze += HandleDesarmMaze;
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

        private void HandleDesarmMaze()
        {
            StartCoroutine(DesarmMaze(desarmMoves));
        }
        
        
        #endregion
        
        #region Methods
        
        private void InitializeMaze()
        {
            _cubeController = new CubeController();//(_SOsManager, maze, generator, mazeView, totalPoints);
            _cubeController.InitializeMaze(mazeSize);
            PieceManager.Initialize(mazeSize);
            //ToDo: _cubeController.OnCellSelected += HandleSelectedCell;
        }
        
        private IEnumerator DesarmMaze(int numberOfMoves)
        {
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