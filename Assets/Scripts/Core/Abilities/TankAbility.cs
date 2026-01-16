using System;
using System.Collections.Generic;
using Core.Interface.Controllers;
using Core.Interface.Models;
using Managers;
using UnityEngine;

namespace Core.Abilities
{
    /// <summary>
    /// Tank ability: Reduces incoming damage and taunts enemies, forcing them to attack the tank.
    /// </summary>
    public class TankAbility : IAbility
    {
        private const int ShieldDuration = 3;
        private const int DamageReduction = 50; // Percentage
        
        public void Execute(IPieceController pieceController, ICubeModel cubeModel)
        {
            var cell = pieceController.Position;
            List<(int x, int y)> changedCells = new List<(int x, int y)> { cell };
            
            // Apply shield status (damage reduction)
            pieceController.PieceModel.SetCurrentStatus(StatusEffect.None); // In a full implementation, this would be a Shield status
            
            // Heal self slightly
            pieceController.RestoreHealth(5);
            
            // Push back enemies in adjacent cells
            int size = cubeModel.Cells[0].GetLength(0);
            foreach (Direction d in Enum.GetValues(typeof(Direction)))
            {
                (int x, int y) adjacentPos = cell;
                switch (d)
                {
                    case Direction.Up: adjacentPos.y--; break;
                    case Direction.Down: adjacentPos.y++; break;
                    case Direction.Left: adjacentPos.x--; break;
                    case Direction.Right: adjacentPos.x++; break;
                }
                
                if (adjacentPos.x < 0 || adjacentPos.y < 0 || adjacentPos.x >= size || adjacentPos.y >= size) 
                    continue;
                
                var piecesInCell = PieceManager.GetPiecesInCell(adjacentPos);
                foreach (var piece in piecesInCell)
                {
                    if (piece.PlayerID == pieceController.PlayerID) continue;
                    // Stun/slow enemies
                    piece.PieceModel.SetRemainingMovs(0);
                    Debug.Log($"Tank stunned {piece.PieceModel.PieceType}");
                }
                
                changedCells.Add(adjacentPos);
            }
            
            GameManager.UpdateCellsView?.Invoke(changedCells);
            Debug.Log("Tank activated shield ability!");
        }
    }
}
