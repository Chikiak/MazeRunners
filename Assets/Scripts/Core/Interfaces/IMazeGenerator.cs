namespace Core.Interfaces
{
    public interface IMazeGenerator
    {
        ICell[,] GenerateFace();
    }
}