using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Core.Interfaces
{
    public abstract class IMazeView : MonoBehaviour
    {
        public Action<(int,int)> OnCellClicked;
        public abstract void InitializeMaze(int size);
        public abstract void UpdateMaze(IMazeFace face);
        public abstract void UpdateRow(int rowIndex, ICell[] rowCells);
        public abstract void UpdateColumn(int columnIndex, ICell[] columnCells);
    }
}