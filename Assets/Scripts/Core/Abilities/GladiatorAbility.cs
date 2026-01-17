using System;
using System.Collections.Generic;
using Core.Interface.Controllers;
using Core.Interface.Models;
using Managers;
using UnityEngine;

namespace Core.Abilities
{
    /// <summary>
    /// Gladiator ability: Closes all walls in current cell, heals self, and damages enemies in the cell.
    /// </summary>
    public class GladiatorAbility : IAbility
    {
        private const int HealAmount = 10;
        private const float DamageMultiplier = 1.5f;
        
        public void Execute(IPieceController pieceController, ICubeModel cubeModel)
        {
            var cell = pieceController.Position;
            List<(int x, int y)> changedCells = new List<(int x, int y)> { cell };
            
            // Close all walls
            foreach (Direction d in Enum.GetValues(typeof(Direction)))
            {
                cubeModel.Cells[0][cell.x, cell.y].SetWall(d, true);
            }
            
            // Heal self
            pieceController.RestoreHealth(HealAmount);
            GameManager.UpdateCellsView?.Invoke(changedCells);
            
            // Damage enemies in cell
            var piecesInCell = PieceManager.GetPiecesInCell(cell);
            if (piecesInCell.Count <= 1) return;

            foreach (var piece in piecesInCell)
            {
                if (piece.PlayerID == pieceController.PlayerID) continue;
                int damage = (int)(pieceController.PieceModel.Damage * DamageMultiplier);
                piece.TakeDamage(damage);
                Debug.Log($"Piece {piece.PieceModel.PieceType} damaged: {piece.PieceModel.Health}");
            }
        }
    }
}
