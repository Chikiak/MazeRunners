using System.Collections.Generic;
using Core.Interface.Models;
using Managers;

namespace Core.Interface.Controllers
{
    public interface IMazeGenerator
    {
        Dictionary<Direction, (int x, int y)> _directionsDelta { get; }
        ICell[,] GenerateMaze(int width, int height, int trapChance, int cycleChance = 20);
        void OpenPathBetweenCells(ICell cell1, ICell cell2);
    }
}