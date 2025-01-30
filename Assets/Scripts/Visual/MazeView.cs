using System;
using System.Collections.Generic;
using Core.Interface.Models;
using Core.Interface.Visual;

namespace Visual
{
    public class MazeView : AMazeView
    {
        
        public override void UpdateMaze(ICell[,] cells)
        {
            for (int i = 0; i < MazeSize; i++)
            for (int j = 0; j < MazeSize; j++)
                cellViews[i,j].UpdateCell(cells[i,j]);
        }

        public override void UpdateCells(List<ICell> cells)
        {
            foreach (var cell in cells)
            {
                cellViews[cell.Position.x, cell.Position.y].UpdateCell(cell);
            }
        }
    }
}