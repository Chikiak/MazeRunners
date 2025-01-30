using Core.Interface.Models;

namespace Core.Interface.Controllers
{
    public interface IMazeGenerator
    {
        ICell[,] GenerateMaze(int width, int height, int trapChance, int cycleChance = 20);
    }
}