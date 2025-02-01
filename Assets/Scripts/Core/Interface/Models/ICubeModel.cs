namespace Core.Interface.Models
{
    public interface ICubeModel
    {
        ICell[][,] Cells { get; }
        
        ICell[] GetRow(int face, int rowIndex);
        ICell[] GetColumn(int face, int columnIndex);
        void SetCell(int face, (int x, int y) position, ICell cell);
        void SetRow(int face, int rowIndex, ICell[] rowCells);
        void SetColumn(int face, int columnIndex, ICell[] columnCells);
        void SetFace(int face, ICell[,] newCells);
    }
}