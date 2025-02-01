using System;
using System.Collections.Generic;
using Core.Interface.Controllers;
using Core.Interface.Models;
using Core.Models;
using Managers;
using UnityEngine;

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
        public Action<List<(int x, int y)>> OnCellsChanged { get; set; }

        public void InitializeMaze(int size)
        {
            Model = new CubeModel(size);
            _mazeGenerator = new MazeGenerator();
            GameManager.SelectingCell += SetSelectableCells;
            GameManager.OnSelectedCell += HandleSelectedCell;
            GameManager.OnAbilityUsed += HandleAblityUsed;
            _size = size;
        }
        public void GenerateMaze()
        {
            for (int i = 0; i < 6; i++)
                Model.SetFace(i, _mazeGenerator.GenerateMaze(_size,_size, 10));
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
            if (GameManager.ActualAction == ActionType.Move && GameManager.GameState != GameStates.PutingInitialPiece) GameManager.Instance.StartCoroutine(StartMov());
        }
        
        #region Abilities
        private void HandleAblityUsed()
        {
            var pieceController = PieceManager.SelectedPiece;

            if (!pieceController.AbilityIsReady())
            {
                Debug.Log($"Ability isnt ready, ready in {pieceController.PieceModel.CurrentCooldown} turns");
                return;
            }

            switch (pieceController.PieceModel.PieceType)
            {
                case PieceType.Healer:
                    HealerAbility(pieceController);
                    break;
                case PieceType.Destroyer:
                    DestroyerAbility(pieceController);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            pieceController.PieceModel.SetCurrentCooldown(pieceController.PieceModel.AbilityCooldown);
        }
        private void HealerAbility(IPieceController pieceController)
        {
            var model = pieceController.PieceModel;
            List<(int x,int y)> cells = GetCellsInRange(model.SpecialRangeType, model.SpecialRange, pieceController.Position);
            
            foreach (var cell in cells)
            {
                Debug.Log($"Healing cell {cell.x}, {cell.y}");
                if (PieceManager.GetPiecesInCell(cell).Count <= 0) continue;

                for (int i = 0; i < PieceManager.GetPiecesInCell(cell).Count; i++)
                {
                    if (PieceManager.PiecesMatrix[cell.x,cell.y][i].PlayerID != pieceController.PlayerID) continue;
                    PieceManager.PiecesMatrix[cell.x,cell.y][i].RestoreHealth(20);
                    Debug.Log($"Piece {PieceManager.PiecesMatrix[cell.x,cell.y][i].PieceModel.PieceType} healed: {PieceManager.PiecesMatrix[cell.x,cell.y][i].PieceModel.Health}");
                }
            }
        }
        private void DestroyerAbility(IPieceController pieceController)
        {
            List<(int x, int y)> changedCells = new List<(int x, int y)>();
            var model = pieceController.PieceModel;
            var cellPos = pieceController.Position;
            changedCells.Add(cellPos);
            var cell = Model.Cells[0][cellPos.x, cellPos.y];
            foreach (Direction d in Enum.GetValues(typeof(Direction)))
            {
                (int x, int y) nextPos = cellPos;
                nextPos.x += _mazeGenerator._directionsDelta[d].x;
                nextPos.y += _mazeGenerator._directionsDelta[d].y;
                if (nextPos.x < 0 || nextPos.y < 0 || nextPos.x >= _size || nextPos.y >= _size) continue;
                var cell2 = Model.Cells[0][nextPos.x, nextPos.y];
                changedCells.Add((nextPos.x, nextPos.y));
                _mazeGenerator.OpenPathBetweenCells(cell, cell2);
            }
            GameManager.UpdateCellsView?.Invoke(changedCells);
        }

        #endregion
    
    }
}