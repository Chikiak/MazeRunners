namespace Core.Interfaces
{
    public interface ITrapsGenerator
    {
        int NumberOfTrapsTypes { get; }
        TrapTypes[,] GetNewTrapMatrix();
    }
}