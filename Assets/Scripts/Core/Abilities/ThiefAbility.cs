using System.Collections.Generic;
using Core.Interface.Controllers;
using Core.Interface.Models;
using Managers;

namespace Core.Abilities
{
    /// <summary>
    /// Thief ability: Collects all points from cells in range.
    /// </summary>
    public class ThiefAbility : IAbility
    {
        public void Execute(IPieceController pieceController, ICubeModel cubeModel)
        {
            int size = cubeModel.Cells[0].GetLength(0);
            List<(int x, int y)> positions = GetCellsManager.GetReachablePositions(
                RangeType.Square, 3, pieceController.Position, size);
            
            foreach ((int x, int y) position in positions)
            {
                ICell cell = cubeModel.Cells[0][position.x, position.y];
                pieceController.PieceModel.SetPoints(pieceController.PieceModel.Points + cell.Points);
                cell.SetPoints(0);
            }
            
            GameManager.UpdateCellsView?.Invoke(positions);
        }
    }
}
