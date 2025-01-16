using Core.Interfaces;
using UnityEngine;

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
            _cells = new ICell[cells.GetLength(0), cells.GetLength(1)];
            SetCells(cells);
        }
        private void InitializeCells(int size)
        {
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    _cells[x, y] = CellFactory.NewCell(TrapTypes.NoTrap,(x,y));
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
                    var cell  = cells[x, y];
                    SetCell(x,y, cell);
                }
            }
        }
        public void SetCell(int columnIndex, int rowIndex, ICell cell)
        {
            _cells[columnIndex, rowIndex] = CellFactory.NewCell(cell);
        }
    }
}