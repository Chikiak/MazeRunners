using Core.Interface.Models;

namespace Core.Models
{
    public class CubeModel : ICubeModel
    {
        private ICell[][,] _cells;
        public ICell[][,] Cells => _cells;
        
        
        public void SetCell(int face, (int x, int y) position, ICell cell)
        {
            _cells[face][position.x, position.y] = new Cell(cell);
        }

        public void SetRow(int face, int rowIndex, ICell[] rowCells)
        {
            for (int i = 0; i < rowCells.Length; i++)
            {
                _cells[face][i, rowIndex] = new Cell(rowCells[i]);
            }
        }

        public void SetColumn(int face, int columnIndex, ICell[] columnCells)
        {
            for (int i = 0; i < columnCells.Length; i++)
            {
                _cells[face][columnIndex, i] = new Cell(columnCells[i]);
            }
        }

        public void SetFace(int face, ICell[,] newCells)
        {
            for (int i = 0; i < newCells.GetLength(0); i++)
            {
                for (int j = 0; j < newCells.GetLength(1); j++)
                {
                    _cells[face][i, j] = new Cell(newCells[i, j]);
                }
            }
        }
    }
}