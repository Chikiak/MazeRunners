using System.Collections.Generic;
using Core.Interface.Controllers;
using Core.Interface.Models;
using Managers;
using UnityEngine;

namespace Core.Abilities
{
    /// <summary>
    /// Healer ability: Heals all allied pieces within range.
    /// </summary>
    public class HealerAbility : IAbility
    {
        private const int HealAmount = 20;
        
        public void Execute(IPieceController pieceController, ICubeModel cubeModel)
        {
            var model = pieceController.PieceModel;
            int size = cubeModel.Cells[0].GetLength(0);
            List<(int x, int y)> cells = GetCellsManager.GetReachablePositions(
                model.SpecialRangeType, model.SpecialRange, pieceController.Position, size);

            foreach (var cell in cells)
            {
                Debug.Log($"Healing cell {cell.x}, {cell.y}");
                var piecesInCell = PieceManager.GetPiecesInCell(cell);
                if (piecesInCell.Count <= 0) continue;

                foreach (var piece in piecesInCell)
                {
                    if (piece.PlayerID != pieceController.PlayerID) continue;
                    piece.RestoreHealth(HealAmount);
                    Debug.Log($"Piece {piece.PieceModel.PieceType} healed: {piece.PieceModel.Health}");
                }
            }
        }
    }
}
