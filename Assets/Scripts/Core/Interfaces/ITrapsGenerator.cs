namespace Core.Interfaces
{
    public interface ITrapsGenerator
    {
        int NumberOfTrapsTypes { get; }
        int[,] GetNewTrapMatrix();
    }
}