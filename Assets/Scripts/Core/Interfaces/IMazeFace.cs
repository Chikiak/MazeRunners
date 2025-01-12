namespace Core.Interfaces
{
    public interface IMazeFace
    {
        ICell[,] Cells { get; }
        void SetCells(ICell[,] cells);
        void SetCell(int columnIndex, int rowIndex, ICell cell);
    }
}