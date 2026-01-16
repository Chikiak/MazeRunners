using System;
using System.Collections.Generic;
using Core.Interface.Controllers;
using Core.Interface.Models;
using Managers;

namespace Core.Abilities
{
    /// <summary>
    /// Destroyer ability: Opens all paths from the current cell.
    /// </summary>
    public class DestroyerAbility : IAbility
    {
        private readonly IMazeGenerator _mazeGenerator;
        
        public DestroyerAbility(IMazeGenerator mazeGenerator)
        {
            _mazeGenerator = mazeGenerator;
        }
        
        public void Execute(IPieceController pieceController, ICubeModel cubeModel)
        {
            List<(int x, int y)> changedCells = new List<(int x, int y)>();
            var cellPos = pieceController.Position;
            int size = cubeModel.Cells[0].GetLength(0);
            changedCells.Add(cellPos);
            var cell = cubeModel.Cells[0][cellPos.x, cellPos.y];
            
            foreach (Direction d in Enum.GetValues(typeof(Direction)))
            {
                (int x, int y) nextPos = cellPos;
                nextPos.x += _mazeGenerator._directionsDelta[d].x;
                nextPos.y += _mazeGenerator._directionsDelta[d].y;
                
                if (nextPos.x < 0 || nextPos.y < 0 || nextPos.x >= size || nextPos.y >= size) continue;
                
                var cell2 = cubeModel.Cells[0][nextPos.x, nextPos.y];
                changedCells.Add((nextPos.x, nextPos.y));
                _mazeGenerator.OpenPathBetweenCells(cell, cell2);
            }

            GameManager.UpdateCellsView?.Invoke(changedCells);
        }
    }
}
