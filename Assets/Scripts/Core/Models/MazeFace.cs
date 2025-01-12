using Core.Interfaces;

namespace Core.Models
{
    public class MazeFace : IMazeFace
    {
        private ICell[,] _cells;
        public ICell[,] Cells => _cells;

        public MazeFace(int size)
        {
            _cells = new ICell[size, size];
            InitializeCells(size);
        }
        public MazeFace(ICell[,] cells)
        {
            _cells = cells;
            SetCells(cells);
        }
        private void InitializeCells(int size)
        {
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    _cells[x, y] = new Cell((x,y));
                }
            }
        }
        public void SetCells(ICell[,] cells)
        {
            int size = cells.GetLength(0);
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    _cells[x, y] = new Cell(cells[x, y]);
                }
            }
        }

        public void SetCell(int columnIndex, int rowIndex, ICell cell)
        {
            _cells[columnIndex, rowIndex] = cell;
        }
    }
}