using System;
using System.Collections.Generic;
using Core.Interface.Controllers;
using Core.Interface.Models;
using Managers;
using UnityEngine;

namespace Core.Controllers
{
    public partial class CubeController
    {
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
                case PieceType.Lancer:
                    LancerAbility(pieceController);
                    break;
                case PieceType.Gladiator:
                    GladiatorAbility(pieceController);
                    break;
                case PieceType.Thief:
                    ThiefAbility(pieceController);
                    break;
                case PieceType.Explorer:
                    ExplorerAbility(pieceController);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            pieceController.PieceModel.SetCurrentCooldown(pieceController.PieceModel.AbilityCooldown);
        }

        private void HealerAbility(IPieceController pieceController)
        {
            var model = pieceController.PieceModel;
            List<(int x, int y)> cells = GetCellsInRange(model.SpecialRangeType, model.SpecialRange, pieceController.Position);

            foreach (var cell in cells)
            {
                Debug.Log($"Healing cell {cell.x}, {cell.y}");
                if (PieceManager.GetPiecesInCell(cell).Count <= 0) continue;

                for (int i = 0; i < PieceManager.GetPiecesInCell(cell).Count; i++)
                {
                    if (PieceManager.PiecesMatrix[cell.x, cell.y][i].PlayerID != pieceController.PlayerID) continue;
                    PieceManager.PiecesMatrix[cell.x, cell.y][i].RestoreHealth(20);
                    Debug.Log(
                        $"Piece {PieceManager.PiecesMatrix[cell.x, cell.y][i].PieceModel.PieceType} healed: {PieceManager.PiecesMatrix[cell.x, cell.y][i].PieceModel.Health}");
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
        private void LancerAbility(IPieceController pieceController)
        {
            var model = pieceController.PieceModel;
            var cellPos = pieceController.Position;
            List<(int x, int y)> cells = GetCellsInRange(model.SpecialRangeType, model.SpecialRange, pieceController.Position);

            foreach (var cell in cells)
            {
                Debug.Log($"Attacking cell {cell.x}, {cell.y}");
                if (PieceManager.GetPiecesInCell(cell).Count <= 0) continue;

                for (int i = 0; i < PieceManager.GetPiecesInCell(cell).Count; i++)
                {
                    if (PieceManager.PiecesMatrix[cell.x, cell.y][i].PlayerID == pieceController.PlayerID) continue;
                    PieceManager.PiecesMatrix[cell.x, cell.y][i].TakeDamage((int)(pieceController.PieceModel.Damage * 1.5f));
                    Debug.Log($"Piece {PieceManager.PiecesMatrix[cell.x, cell.y][i].PieceModel.PieceType} damaged: {PieceManager.PiecesMatrix[cell.x, cell.y][i].PieceModel.Health}");
                }
            }
        }
        private void GladiatorAbility(IPieceController pieceController)
        {
            var model = pieceController.PieceModel;
            var cell = pieceController.Position;
            List<(int x, int y)> changedCells = new List<(int x, int y)>();
            changedCells.Add(cell);
            foreach (Direction d in Enum.GetValues(typeof(Direction)))
            {
                Model.Cells[0][cell.x, cell.y].SetWall(d, true);
            }
            
            pieceController.RestoreHealth(10);
            GameManager.UpdateCellsView?.Invoke(changedCells);
            
            if (PieceManager.GetPiecesInCell(cell).Count <= 1) return;

            for (int i = 0; i < PieceManager.GetPiecesInCell(cell).Count; i++)
            {
                if (PieceManager.PiecesMatrix[cell.x, cell.y][i].PlayerID == pieceController.PlayerID) continue;
                PieceManager.PiecesMatrix[cell.x, cell.y][i].TakeDamage((int)(pieceController.PieceModel.Damage * 1.5f));
                Debug.Log($"Piece {PieceManager.PiecesMatrix[cell.x, cell.y][i].PieceModel.PieceType} damaged: {PieceManager.PiecesMatrix[cell.x, cell.y][i].PieceModel.Health}");
            }
            
        }
        private void ThiefAbility(IPieceController pieceController)
        {
            ICell cell;
            List<(int x, int y)> positions = GetCellsManager.GetReachablePositions(RangeType.Square, 3, pieceController.Position, _size);
            foreach ((int x, int y) position in positions)
            {
                cell = Model.Cells[0][position.x, position.y];
                pieceController.PieceModel.SetPoints(pieceController.PieceModel.Points + cell.Points);
                cell.SetPoints(0);
            }
        }

        private void ExplorerAbility(IPieceController pieceController)
        {
            //ToDo
        }

        #endregion
    }
}