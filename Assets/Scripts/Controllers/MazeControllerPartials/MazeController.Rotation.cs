using System;
using Core.Interfaces;
using Core.Models;

namespace Controllers
{
    public partial class MazeController
    {
        #region Rotation
        public void RotateFace(bool horizontal, int rotateIndex, bool clockwise)
        {
            if (rotateIndex < 0 || rotateIndex >= _maze.Size) return;
            if (rotateIndex == 0 || rotateIndex == _maze.Size - 1)
            {
                RotateExtremeFace(horizontal, rotateIndex, clockwise);
            }

            int[] facesI;
            if (horizontal)
            {
                facesI = clockwise ? new[] { 0, 1, 5, 3 } : new[] { 0, 3, 5, 1 };
            }
            else
            {
                facesI = clockwise ? new[] { 0, 4, 5, 2 } : new[] { 0, 2, 5, 4 };
            }

            SimpleRotate(horizontal, rotateIndex, facesI);
        }

        private void SimpleRotate(bool horizontal, int rotateIndex, int[] facesI)
        {
            if (facesI.Length != 4) return;
            ICell[][] tempCells =
            {
                new ICell[_maze.Size],
                new ICell[_maze.Size],
                new ICell[_maze.Size],
                new ICell[_maze.Size]
            };

            if (horizontal)
            {
                for (int i = 0; i < facesI.Length; i++)
                {
                    for (int j = 0; j < _maze.Size; j++)
                    {
                        var newCell = CellFactory.NewCell(GetMazeFace(facesI[i]).Cells[j, rotateIndex]);
                        tempCells[i][j] = newCell;
                    }
                }

                for (int i = 0; i < facesI.Length - 1; i++)
                {
                    for (int j = 0; j < _maze.Size; j++)
                    {
                        var face = GetMazeFace(facesI[i]);
                        var newCell = tempCells[i + 1][j];
                        face.SetCell(j, rotateIndex, CellFactory.NewCell(newCell));
                    }
                }

                for (int i = 0; i < _maze.Size; i++)
                {
                    GetMazeFace(facesI[facesI.Length - 1]).SetCell(i, rotateIndex, CellFactory.NewCell(tempCells[0][i]));
                }
            }
            else
            {
                for (int i = 0; i < facesI.Length; i++)
                {
                    for (int j = 0; j < _maze.Size; j++)
                    {
                        var newCell = CellFactory.NewCell(GetMazeFace(facesI[i]).Cells[rotateIndex, j]);
                        tempCells[i][j] = newCell;
                    }
                }

                for (int i = 0; i < facesI.Length - 1; i++)
                {
                    for (int j = 0; j < _maze.Size; j++)
                    {
                        var face = GetMazeFace(facesI[i]);
                        var newCell = tempCells[i + 1][j];
                        face.SetCell(rotateIndex, j, newCell);
                    }
                }

                for (int i = 0; i < _maze.Size; i++)
                {
                    GetMazeFace(facesI[facesI.Length - 1]).SetCell(rotateIndex, i, tempCells[0][i]);
                }
            }
        }

        private void RotateExtremeFace(bool horizontal, int rotateIndex, bool clockwise)
        {
            //This Work Only for 6 Faces
            int faceToRotate;
            if (horizontal)
            {
                faceToRotate = (rotateIndex == 0) ? 4 : 2;
            }
            else
            {
                faceToRotate = (rotateIndex == 0) ? 1 : 3;
            }

            RotateCellsInFace(clockwise, faceToRotate);
        }

        private void RotateCellsInFace(bool clockwise, int faceIndex)
        {
            var face = GetMazeFace(faceIndex);
            ICell[,] matrix = face.Cells;
            int n = matrix.GetLength(0);
            ICell[,] temp = new ICell[n, n];

            if (clockwise)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        temp[n - 1 - j, i] = matrix[i, j];
                        RotateCellWalls(temp[n - 1 - j, i], clockwise);
                    }
                }
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        temp[j, n - 1 - i] = matrix[i, j];
                        RotateCellWalls(temp[j, n - 1 - i], clockwise);
                    }
                }
            }
            
            _maze.SetFace(faceIndex, temp);
        }

        private void RotateCellWalls(ICell cell, bool clockwise)
        {
            bool[] currentWalls = new bool[4];
            for (int i = 0; i < 4; i++)
            {
                currentWalls[i] = cell.Walls[i];
            }

            bool[] newWalls = new bool[4];
            if (clockwise)
            {
                // up -> right -> down -> left -> up
                newWalls[(int)Directions.Up] = currentWalls[(int)Directions.Left];
                newWalls[(int)Directions.Right] = currentWalls[(int)Directions.Up];
                newWalls[(int)Directions.Down] = currentWalls[(int)Directions.Right];
                newWalls[(int)Directions.Left] = currentWalls[(int)Directions.Down];
            }
            else
            {
                // up -> left -> down -> right -> up
                newWalls[(int)Directions.Up] = currentWalls[(int)Directions.Right];
                newWalls[(int)Directions.Right] = currentWalls[(int)Directions.Down];
                newWalls[(int)Directions.Down] = currentWalls[(int)Directions.Left];
                newWalls[(int)Directions.Left] = currentWalls[(int)Directions.Up];
            }

            foreach (var d in Enum.GetValues(typeof(Directions)))
            {
                cell.SetWall((Directions)d, newWalls[(int)d]);
            }
        }

        #endregion
    }
}