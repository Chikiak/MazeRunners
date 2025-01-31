using System;
using Core.Interface.Models;
using Managers;

namespace Core.Controllers
{
    public partial class CubeController
    {
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
            if (index == 0 || index == _size - 1) RotateExtremeFace(isRow, index, clockwise);
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

                for (int i = 0; i < facesI.Length - 1; i++)
                {
                    Model.SetRow(facesI[i + 1], index, tempCells[i]);
                }

                Model.SetRow(facesI[0], index, tempCells[^1]);
            }
            else
            {
                for (int i = 0; i < facesI.Length; i++)
                {
                    tempCells[i] = Model.GetColumn(facesI[i], index);
                }

                for (int i = 0; i < facesI.Length - 1; i++)
                {
                    Model.SetColumn(facesI[i + 1], index, tempCells[i]);
                }

                Model.SetColumn(facesI[0], index, tempCells[^1]);
            }
        }
        private void RotateExtremeFace(bool horizontal, int rotateIndex, bool clockwise)
        {
            //This Work Only for 6 Faces
            int faceToRotate;
            if (horizontal)
            {
                if (rotateIndex == 0)
                {
                    faceToRotate = 4;
                    RotateCellsInFace(clockwise, faceToRotate);
                }
                else
                {
                    faceToRotate = 2;
                    RotateCellsInFace(!clockwise, faceToRotate);
                }
            }
            else
            {
                faceToRotate = (rotateIndex == 0) ? 1 : 3;
                if (rotateIndex == 0)
                {
                    faceToRotate = 1;
                    RotateCellsInFace(clockwise, faceToRotate);
                }
                else
                {
                    faceToRotate = 3;
                    RotateCellsInFace(!clockwise, faceToRotate);
                }
            }
        }
        private void RotateCellsInFace(bool clockwise, int faceIndex)
        {
            ICell[][] rotatedFace = new ICell[_size][];
            for (int i = 0; i < rotatedFace.Length; i++)
            {
                rotatedFace[i] = Model.GetRow(faceIndex, i);
            }

            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    rotatedFace[i][j].RotateWalls(clockwise);
                }
            }

            for (int i = 0; i < _size; i++)
            {
                if (clockwise)
                {
                    Model.SetColumn(faceIndex, _size - 1 - i, rotatedFace[i]);
                }
                else
                {
                    var tempColumn = rotatedFace[i];
                    ICell[] newColumn = new ICell[_size];
                    for (int k = 0; k < _size; k++)
                    {
                        newColumn[k] = tempColumn[^(k + 1)];
                    }

                    Model.SetColumn(faceIndex, i, newColumn);
                }
            }
        }

        #endregion
    }
}