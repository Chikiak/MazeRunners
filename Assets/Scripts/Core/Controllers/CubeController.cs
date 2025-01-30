using System;
using Core.Interface.Controllers;
using Core.Interface.Models;
using Core.Models;
using Managers;

namespace Core.Controllers
{
    public class CubeController : ICubeController
    {
        public ICubeModel Model { get; private set; }
        private IMazeGenerator _mazeGenerator;
        private int _size;
        public void InitializeMaze(int size)
        {
            Model = new CubeModel(size);
            _mazeGenerator = new MazeGenerator();
            _size = size;
        }

        #region Rotation
        public void Rotate(bool isRow, bool clockwise, int index)
        {
            if (index < 0 || index >= _size) throw new Exception($"Rotate: Invalid index: {index}");
            int[] facesI;
            if (isRow)
            {
                facesI = clockwise ? new[] { 0, 3, 5, 1 } : new[] { 0, 1, 5, 3 };
            }
            else
            {
                facesI = clockwise ? new[] { 0, 2, 5, 4 } : new[] { 0, 4, 5, 2 };
            }
            SimpleRotate(facesI, isRow, index);
        }

        private void SimpleRotate(int[] facesI, bool isRow, int index)
        {
            ICell[][] tempCells =
            {
                new ICell[_size],
                new ICell[_size],
                new ICell[_size],
                new ICell[_size],
            };
            if (isRow)
            {
                for (int i = 0; i < facesI.Length; i++)
                {
                    tempCells[i] = Model.GetRow(facesI[i], index);
                }

                for (int i = 0; i < facesI.Length-1; i++)
                {
                    Model.SetRow(facesI[i+1], index, tempCells[i]);
                }
                Model.SetRow(facesI[0], index, tempCells[^1]);
            }
            else
            {
                for (int i = 0; i < facesI.Length; i++)
                {
                    tempCells[i] = Model.GetColumn(facesI[i], index);
                }

                for (int i = 0; i < facesI.Length-1; i++)
                {
                    Model.SetColumn(facesI[i+1], index, tempCells[i]);
                }
                Model.SetColumn(facesI[0], index, tempCells[^1]);
            }
            
        }
        
        #endregion

        public void GenerateMaze()
        {
            for (int i = 0; i < 6; i++)
                Model.SetFace(i, _mazeGenerator.GenerateMaze(_size,_size, 0));
        }
    }
}