using System.Collections.Generic;
using Core.Interface.Models;
using Managers;
using UnityEngine;
using UnityEngine.UI;
using Visual;

namespace Core.Interface.Visual
{
    public abstract class AMazeView : MonoBehaviour
    {
        [SerializeField] protected GameObject cellPrefab;
        [SerializeField] protected Transform mazeContainer;
        private MazeRotationArrows rotationButtons;
        
        protected CellView[,] cellViews;

        protected int MazeSize;

        public virtual void InitializeMaze(int size)
        {
            MazeSize = size;
            cellViews = new CellView[MazeSize, MazeSize];
            InstanceCellViews();
        }

        private void InstanceCellViews()
        {
            rotationButtons = GetComponent<MazeRotationArrows>();
            rotationButtons.InitializeArrows(MazeSize);
            
            var rectTransform = mazeContainer.GetComponent<RectTransform>();
            float containerSize = MazeSize * 64f;
            rectTransform.sizeDelta = new Vector2(containerSize, containerSize);

            var grid = mazeContainer.GetComponent<GridLayoutGroup>();
            grid.constraintCount = MazeSize;

            for (int y = 0; y < MazeSize; y++)
            for (int x = 0; x < MazeSize; x++)
                CreateCell(x, y);
        }

        private void CreateCell(int x, int y)
        {
            GameObject cellObject = Instantiate(cellPrefab, mazeContainer);
            cellObject.name = $"Cell_{x}_{y}";

            cellViews[x, y] = cellObject.GetComponent<CellView>(); 
            //cellViews[x, y].OnCellClicked += HandleCellClicked;
        }
        public abstract void UpdateMaze(ICell[,] cells);
        public abstract void UpdateCells(List<ICell> cells);
    }
}