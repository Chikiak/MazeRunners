using System;
using System.Collections.Generic;
using Core.Interfaces;

namespace Controllers
{
    public partial class MazeController
    {
        #region GetCells
        private List<ICell> GetCellsInRange(RangesType rangeType, int size, (int, int) pos)
        {
            List<ICell> cells = new List<ICell>();
            switch (rangeType)
            {
                case RangesType.Square:
                    cells = GetCellsInSquareRange(size, pos);
                    break;
                case RangesType.Path:
                    cells = GetCellsInPathRange(size, pos);
                    break;
                default:
                    throw new Exception("Invalid range type");
            }

            return cells;
        }
        private List<ICell> GetCellsInSquareRange(int size, (int, int) pos)
        {
            if (size % 2 == 0) throw new ArgumentException("Size cant be divisible by 2");
            List<ICell> cells = new List<ICell>();

            int iniX = Math.Max(0, pos.Item1 - (size - 1) / 2);
            int iniY = Math.Max(0, pos.Item2 - (size - 1) / 2);
            int finX = Math.Min(_maze.Size - 1, pos.Item1 + (size - 1) / 2);
            int finY = Math.Min(_maze.Size - 1, pos.Item2 + (size - 1) / 2);

            for (int x = iniX; x <= finX; x++)
            {
                for (int y = iniY; y <= finY; y++)
                {
                    cells.Add(GetMazeFace(0).Cells[x, y]);
                }
            }

            return cells;
        }
        private List<ICell> GetCellsInPathRange(int range, (int, int) pos)
        {
            List<ICell> cells = new List<ICell>();
            var mazeFace = GetMazeFace(0);
            var startCell = mazeFace.Cells[pos.Item1, pos.Item2];

            // Usar un queue para BFS (Breadth-First Search)
            Queue<(ICell cell, int distance)> queue = new Queue<(ICell cell, int distance)>();
            HashSet<(int, int)> visited = new HashSet<(int, int)>();

            queue.Enqueue((startCell, 0));
            visited.Add(pos);

            while (queue.Count > 0)
            {
                var (currentCell, distance) = queue.Dequeue();
                cells.Add(currentCell);

                // Si alcanzamos el límite de rango, no exploramos más desde esta celda
                if (distance >= range) continue;

                // Revisar cada dirección
                foreach (Directions direction in Enum.GetValues(typeof(Directions)))
                {
                    // Verificar si hay un camino abierto en esta dirección
                    if (!IsPathOpen(currentCell, direction)) continue;

                    // Calcular nueva posición
                    var newPos = direction switch
                    {
                        Directions.Up => (currentCell.Position.Item1, currentCell.Position.Item2 - 1),
                        Directions.Down => (currentCell.Position.Item1, currentCell.Position.Item2 + 1),
                        Directions.Left => (currentCell.Position.Item1 - 1, currentCell.Position.Item2),
                        Directions.Right => (currentCell.Position.Item1 + 1, currentCell.Position.Item2),
                        _ => currentCell.Position
                    };

                    // Verificar si la posición es válida y no ha sido visitada
                    if (newPos.Item1 < 0 || newPos.Item1 >= _maze.Size ||
                        newPos.Item2 < 0 || newPos.Item2 >= _maze.Size ||
                        visited.Contains(newPos))
                        continue;

                    visited.Add(newPos);
                    var nextCell = mazeFace.Cells[newPos.Item1, newPos.Item2];
                    queue.Enqueue((nextCell, distance + 1));
                }
            }

            // Remover la celda inicial de la lista
            cells.RemoveAt(0);
            return cells;
        }
        private bool IsPathOpen(ICell cell, Directions direction)
        {
            var mazeFace = GetMazeFace(0);
            if (cell.Walls[(int)direction]) return false;
            var newPosX = cell.Position.Item1;
            if (direction == Directions.Left) newPosX--;
            else if (direction == Directions.Right) newPosX++;
            var newPosY = cell.Position.Item2;
            if (direction == Directions.Up) newPosY--;
            else if (direction == Directions.Down) newPosY++;
            if (newPosX < 0 || newPosX >= _maze.Size) return false;
            if (newPosY < 0 || newPosY >= _maze.Size) return false;

            Directions adjDirection = direction switch
            {
                Directions.Up => Directions.Down,
                Directions.Down => Directions.Up,
                Directions.Left => Directions.Right,
                Directions.Right => Directions.Left,
                _ => throw new Exception("Invalid direction")
            };

            var adjCell = mazeFace.Cells[newPosX, newPosY];
            if (adjCell.Walls[(int)adjDirection]) return false;

            return true;
        }

        #endregion
    }
}