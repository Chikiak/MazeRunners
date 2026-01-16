using System.Collections.Generic;
using Core.Interface.Controllers;
using Core.Interface.Models;
using Managers;
using UnityEngine;

namespace Core.Abilities
{
    /// <summary>
    /// Lancer ability: Deals damage to enemies in a line pattern.
    /// </summary>
    public class LancerAbility : IAbility
    {
        private const float DamageMultiplier = 1.5f;
        
        public void Execute(IPieceController pieceController, ICubeModel cubeModel)
        {
            var model = pieceController.PieceModel;
            int size = cubeModel.Cells[0].GetLength(0);
            List<(int x, int y)> cells = GetCellsManager.GetReachablePositions(
                model.SpecialRangeType, model.SpecialRange, pieceController.Position, size);

            foreach (var cell in cells)
            {
                Debug.Log($"Attacking cell {cell.x}, {cell.y}");
                var piecesInCell = PieceManager.GetPiecesInCell(cell);
                if (piecesInCell.Count <= 0) continue;

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
}
