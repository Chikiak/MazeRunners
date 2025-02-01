using System;
using System.Collections.Generic;
using Core.Interface.Controllers;
using Core.Interface.Models;
using Core.Models;
using Managers;

namespace Core.Controllers
{
    public class MazeGenerator : IMazeGenerator
    {
        public Dictionary<Direction, (int x, int y)> _directionsDelta { get; }

        public ICell[,] GenerateMaze(int width, int height, int trapChance, int cycleChance)
        {
            _width = width;
            _height = height;
            _cycleChance = cycleChance;
            _trapGenerator = new TrapGenerator(width, _random, 3, trapChance);
            
            return GenerateFace();
        }

        private TrapGenerator _trapGenerator;
        private Random _random;
        private int _cycleChance;
        private int[] _dx = { 0, 0, -1, 1 };
        private int[] _dy = { -1, 1, 0, 0 };

        private int _width;
        private int _height;

        public MazeGenerator()
        {
            _random = new Random();
            
            _directionsDelta = new Dictionary<Direction, (int x, int y)>();
            _directionsDelta[Direction.Up] = (-1, 0);
            _directionsDelta[Direction.Down] = (1, 0);
            _directionsDelta[Direction.Left] = (0, -1);
            _directionsDelta[Direction.Right] = (0, 1);
        }
        
        public ICell[,] GenerateFace()
        {
            TrapType[,] trapMatrix = _trapGenerator.GetNewTrapMatrix();
            var cells = new ICell[_width, _height];
            var visited = new bool[_width, _height];

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    cells[x, y] = trapMatrix[x, y] switch
                    {
                        TrapType.Nothing => new Cell((x, y), TrapType.Nothing),
                        TrapType.Spikes => new Cell((x, y), TrapType.Spikes),
                        TrapType.Teleport => new Cell((x, y), TrapType.Teleport),
                        TrapType.AffectStats => new Cell((x, y), TrapType.AffectStats),
                        _ => throw new Exception($"Unexpected trap type {trapMatrix[x, y]}")
                    };
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
            
            Direction[] directions = { Direction.Down,Direction.Left,Direction.Right,Direction.Up };
            ShuffleArray(directions);

            foreach (var direction in directions)
            {
                (int,int) newPosition = (x + _directionsDelta[direction].x ,y + _directionsDelta[direction].y);
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
                    Direction[] directions = { Direction.Down,Direction.Left,Direction.Right,Direction.Up };
                    ShuffleArray(directions);

                    foreach (var direction in directions)
                    {
                        (int,int) newPosition = (cell.Position.x + _directionsDelta[direction].x,cell.Position.y + _directionsDelta[direction].y);
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
        private void ShuffleArray(Direction[] array)
        {
            int x = _random.Next(array.Length * 2);
            for (int i = 0; i < x; i++)
            {
                int h = _random.Next(array.Length -1);
                var temp = array[h];
                array[h] = array[i % array.Length];
                array[i % array.Length] = temp;
            }
        }
        public void OpenPathBetweenCells(ICell cell1, ICell cell2)
        {
            if (cell1 == null || cell2 == null) return;
            var position1 = cell1.Position;
            var position2 = cell2.Position;
            foreach (var d in Enum.GetValues(typeof(Direction)))
            {
                int x = position1.Item1 + _dx[(int)d];
                int y = position1.Item2 + _dy[(int)d];
                (int,int) newPosition = (x,y);
                if (!ValidatePosition(newPosition)) continue;
                if (newPosition == position2)
                {
                    cell1.SetWall((Direction)d, false);
                    if ((int)d % 2 == 1)
                    {
                        cell2.SetWall((Direction)(int)d - 1, false);
                    }
                    else
                    {
                        cell2.SetWall((Direction)(int)d + 1, false);
                    }

                    return;
                }
            }
        }
        private bool ValidatePosition((int x, int y) position)
        {
            if (position.x < 0 || position.y < 0) return false;
            if (position.x >= _width || position.y >= _height) return false;
            return true;
        }
    }
}