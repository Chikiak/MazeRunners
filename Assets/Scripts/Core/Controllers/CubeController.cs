using System;
using System.Collections.Generic;
using Core.Interface.Controllers;
using Core.Interface.Models;
using Core.Models;
using Managers;
using UnityEngine;
using Random = System.Random;

namespace Core.Controllers
{
    public partial class CubeController : ICubeController
    {
        public ICubeModel Model { get; private set; }
        private IMazeGenerator _mazeGenerator;
        private int _size;
        private List<(int x, int y)> _selectableCells;
        private List<List<Direction>> _directionsToCells;
        private int _directionIndex;
        private int _remainingPoints;
        public Action<List<(int x, int y)>> OnCellsChanged { get; set; }

        public void InitializeMaze(int size, int totalPoints)
        {
            _remainingPoints = totalPoints;
            Model = new CubeModel(size);
            _mazeGenerator = new MazeGenerator();
            GameManager.SelectingCell += SetSelectableCells;
            GameManager.OnSelectedCell += HandleSelectedCell;
            GameManager.OnAbilityUsed += HandleAblityUsed;
            PieceManager.OnDefeated += PointsPieceToPosition;
            _size = size;
        }

        public void GenerateMaze()
        {
            for (int i = 0; i < 6; i++)
                Model.SetFace(i, _mazeGenerator.GenerateMaze(_size, _size, 10));
            
            InitializePoints();
        }

        private void HandleSelectedCell()
        {
            for (int i = 0; i < _selectableCells.Count; i++)
            {
                var cell = _selectableCells[i];
                Model.Cells[0][cell.x, cell.y].SetSelectable(false);
                if (cell == GameManager.SelectedCell) _directionIndex = i;
            }

            OnCellsChanged?.Invoke(_selectableCells);
            _selectableCells.Clear();
            if (GameManager.ActualAction == ActionType.Move && GameManager.GameState != GameStates.PutingInitialPiece)
                GameManager.Instance.StartCoroutine(StartMov());
        }
    }
}