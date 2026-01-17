using System.Collections.Generic;
using Core.Interface.Controllers;
using Core.Interface.Models;
using Managers;
using UnityEngine;

namespace Core.Abilities
{
    /// <summary>
    /// Archer ability: Deals damage to a single enemy at long range.
    /// </summary>
    public class ArcherAbility : IAbility
    {
        private const float DamageMultiplier = 2.0f;
        
        public void Execute(IPieceController pieceController, ICubeModel cubeModel)
        {
            var model = pieceController.PieceModel;
            int size = cubeModel.Cells[0].GetLength(0);
            List<(int x, int y)> cells = GetCellsManager.GetReachablePositions(
                model.SpecialRangeType, model.SpecialRange, pieceController.Position, size);

            foreach (var cell in cells)
            {
                var piecesInCell = PieceManager.GetPiecesInCell(cell);
                if (piecesInCell.Count <= 0) continue;

                foreach (var piece in piecesInCell)
                {
                    if (piece.PlayerID == pieceController.PlayerID) continue;
                    int damage = (int)(pieceController.PieceModel.Damage * DamageMultiplier);
                    piece.TakeDamage(damage);
                    Debug.Log($"Archer hit {piece.PieceModel.PieceType} for {damage} damage. Health: {piece.PieceModel.Health}");
                    return; // Only hit one target
                }
            }
        }
    }
}
