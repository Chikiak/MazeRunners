using System.Collections.Generic;
using Core.Interface.Controllers;
using Core.Interface.Models;
using Managers;
using UnityEngine;

namespace Core.Abilities
{
    /// <summary>
    /// Explorer ability: Reveals trap information in range and grants temporary speed boost.
    /// </summary>
    public class ExplorerAbility : IAbility
    {
        private const int SpeedBoost = 2;
        
        public void Execute(IPieceController pieceController, ICubeModel cubeModel)
        {
            var model = pieceController.PieceModel;
            int size = cubeModel.Cells[0].GetLength(0);
            
            // Grant speed boost
            int newSpeed = model.Speed + SpeedBoost;
            if (newSpeed > model.MaxSpeed + SpeedBoost)
                newSpeed = model.MaxSpeed + SpeedBoost;
            model.SetSpeed(newSpeed);
            model.SetRemainingMovs(newSpeed);
            
            // Reveal traps in diamond range
            List<(int x, int y)> cells = GetCellsManager.GetReachablePositions(
                RangeType.Diamond, 3, pieceController.Position, size);

            foreach (var cellPos in cells)
            {
                var cell = cubeModel.Cells[0][cellPos.x, cellPos.y];
                if (cell.Trap.TrapType != TrapType.Nothing)
                {
                    Debug.Log($"Trap revealed at ({cellPos.x}, {cellPos.y}): {cell.Trap.TrapType}");
                }
            }
            
            GameManager.UpdateCellsView?.Invoke(cells);
        }
    }
}
