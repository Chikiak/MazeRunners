using Core.Interface.Models;

namespace Core.Models
{
    public class CubeModel : ICubeModel
    {
        public ICell[][,] Cells { get; private set; }
        private int _size;

        public CubeModel( int size)
        {
            _size = size;
            Cells = new ICell[6][,];
            for (int i = 0; i < 6; i++)
                Cells[i] = new ICell[size,size];
            
        }
        
        public ICell[] GetRow(int face, int rowIndex)
        {
            ICell[] rowCells = new ICell[_size];
            for (int i = 0; i < _size; i++)
            {
                rowCells[i] = new Cell(Cells[face][i, rowIndex]);
            }
            return rowCells;
        }

        public ICell[] GetColumn(int face, int columnIndex)
        {
            ICell[] columnCells = new ICell[_size];
            for (int i = 0; i < _size; i++)
            {
                columnCells[i] = new Cell(Cells[face][columnIndex, i]);
            }
            return columnCells;
        }
        
        public void SetCell(int face, (int x, int y) position, ICell cell)
        {
            Cells[face][position.x, position.y] = new Cell(cell);
        }

        public void SetRow(int face, int rowIndex, ICell[] rowCells)
        {
            for (int i = 0; i < rowCells.Length; i++)
            {
                Cells[face][i, rowIndex] = new Cell(rowCells[i]);
            }
        }

        public void SetColumn(int face, int columnIndex, ICell[] columnCells)
        {
            for (int i = 0; i < columnCells.Length; i++)
            {
                Cells[face][columnIndex, i] = new Cell(columnCells[i]);
            }
        }

        public void SetFace(int face, ICell[,] newCells)
        {
            for (int i = 0; i < newCells.GetLength(0); i++)
            {
                for (int j = 0; j < newCells.GetLength(1); j++)
                {
                    Cells[face][i, j] = new Cell(newCells[i, j]);
                }
            }
        }
    }
}