using System;
using Core.Interfaces;
using Core.Models;
using Core.Models.Traps;
using UnityEngine;
using Random = System.Random;

namespace Core.Generators
{
    public class MazeGenerator : IMazeGenerator
    {
        private Random _random;
        private int _cycleChance;
        private int[] _dx = { 0, 0, -1, 1 };
        private int[] _dy = { -1, 1, 0, 0 };
        private int _size;
        private TrapsGenerator _trapsGenerator;

        public MazeGenerator(int size, int trapChance, int cycleChance = 10)
        {
            _random = new Random();
            _cycleChance = cycleChance;
            _size = size;
            _trapsGenerator = new TrapsGenerator(size, _random, 1, trapChance);
        }

        #region GenerateFace
        public ICell[,] GenerateFace()
        {
            TrapTypes[,] trapMatrix = _trapsGenerator.GetNewTrapMatrix();
            int size = _size;
            var cells = new ICell[size, size];
            var visited = new bool[size, size];

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    cells[x, y] = CellFactory.NewCell(trapMatrix[x, y], (x, y));
                }
            }
            
            RecursiveMaze(0, 0, cells, visited);
            CreateCycles(cells);
            return cells;
        }
        private void RecursiveMaze(int x, int y, ICell[,] cells, bool[,] visited)
        {
            ICell currentCell = cells[x, y];
            visited[x, y] = true;
            
            int[] directions = {0,1,2,3};
            ShuffleIntArray(directions);

            foreach (var direction in directions)
            {
                (int,int) newPosition = (x + _dx[direction],y + _dy[direction]);
                if (ValidatePosition(newPosition) && !visited[newPosition.Item1, newPosition.Item2])
                {
                    ICell newCell = cells[newPosition.Item1, newPosition.Item2];
                    OpenPathBetweenCells(currentCell, newCell);
                    RecursiveMaze(newPosition.Item1, newPosition.Item2, cells, visited);
                }
            }
        }
        private void CreateCycles(ICell[,] cells)
        {
            foreach (ICell cell in cells)
            {
                int r = _random.Next(0, 100);
                if (r < _cycleChance)
                {
                    int[] directions = {0,1,2,3};
                    ShuffleIntArray(directions);

                    foreach (var direction in directions)
                    {
                        (int,int) newPosition = (cell.Position.Item1 + _dx[direction],cell.Position.Item2 + _dy[direction]);
                        if (ValidatePosition(newPosition) && cell.Walls[direction])
                        {
                            ICell newCell = cells[newPosition.Item1, newPosition.Item2];
                            OpenPathBetweenCells(cell, newCell);
                            break;
                        }
                    }
                }
            }
        }
        private void ShuffleIntArray(int[] array)
        {
            int x = _random.Next(array.Length * 2);
            for (int i = 0; i < x; i++)
            {
                int h = _random.Next(array.Length -1);
                int temp = array[h];
                array[h] = array[i % array.Length];
                array[i % array.Length] = temp;
            }
        }
        private void OpenPathBetweenCells(ICell cell1, ICell cell2)
        {
            if (cell1 == null || cell2 == null) return;
            var position1 = cell1.Position;
            var position2 = cell2.Position;
            foreach (var d in Enum.GetValues(typeof(Directions)))
            {
                int x = position1.Item1 + _dx[(int)d];
                int y = position1.Item2 + _dy[(int)d];
                (int,int) newPosition = (x,y);
                if (position1.Item1 == 0 && position1.Item2 == 0)
                {
                    Console.WriteLine($"{(Directions) d} : {(int) d}");
                }
                
                if (!ValidatePosition(newPosition)) continue;
                if (newPosition == position2)
                {
                    cell1.SetWall((Directions)d, false);
                    if ((int)d % 2 == 1)
                    {
                        cell2.SetWall((Directions)(int)d - 1, false);
                    }
                    else
                    {
                        cell2.SetWall((Directions)(int)d + 1, false);
                    }

                    return;
                }
            }
        }
        private bool ValidatePosition((int, int) position)
        {
            if (position.Item1 < 0 || position.Item2 < 0) return false;
            if (position.Item1 >= _size || position.Item2 >= _size) return false;
            return true;
        }
        #endregion
    }
}