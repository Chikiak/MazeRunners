using System;
using Core.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class MazeView : IMazeView
    {
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private Transform mazeContainer;
        [SerializeField] private MazeRotationArrows rotationButtons;

        private CellView[,] _cellViews;
        private int _mazeSize;

        public override void InitializeMaze(int size)
        {
            _mazeSize = size;
            _cellViews = new CellView[size, size];
            
            rotationButtons.InitializeArrows(size);
            
            
            var rectTransform = mazeContainer.GetComponent<RectTransform>();
            float containerSize = size * 64f;
            rectTransform.sizeDelta = new Vector2(containerSize, containerSize);

            var grid = mazeContainer.GetComponent<GridLayoutGroup>();
            grid.constraintCount = _mazeSize;

            for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
                CreateCell(x, y);
        }
        private void CreateCell(int x, int y)
        {
            GameObject cellObject = Instantiate(cellPrefab, mazeContainer);
            cellObject.name = $"Cell_{x}_{y}";

            _cellViews[x, y] = cellObject.GetComponent<CellView>();
            _cellViews[x, y].OnCellClicked += HandleCellClicked;
        }

        private void HandleCellClicked((int, int) position)
        {
            OnCellClicked?.Invoke(position);
        }

        public override void UpdateMaze(IMazeFace mazeFace)
        {
            ICell[,] cells = mazeFace.Cells;
            for (int x = 0; x < _mazeSize; x++)
            for (int y = 0; y < _mazeSize; y++)
                if (cells[x, y] != null && _cellViews[x, y] != null)
                    _cellViews[x, y].UpdateCell(cells[x, y]);
        }

        public override void UpdateRow(int rowIndex, ICell[] rowCells)
        {
            for (int x = 0; x < _mazeSize; x++)
            {
                if (rowCells[x] != null && _cellViews[x, rowIndex] != null)
                {
                    _cellViews[x, rowIndex].UpdateCell(rowCells[x]);
                }
            }
        }

        public override void UpdateColumn(int columnIndex, ICell[] columnCells)
        {
            for (int y = 0; y < _mazeSize; y++)
            {
                if (columnCells[y] != null && _cellViews[columnIndex, y] != null)
                {
                    _cellViews[columnIndex, y].UpdateCell(columnCells[y]);
                }
            }
        }
        
        
    }
}