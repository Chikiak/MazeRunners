using System;
using System.Collections.Generic;
using Core.Interface.Models;
using Managers;

namespace Core.Models
{
    public class Cell : ICell
    {
        public (int x, int y) Position { get; private set; }
        public int Points { get; private set; }
        public Dictionary<Direction, bool> Walls { get; private set; }
        private ITrap _trap;
        public ITrap Trap => _trap;

        public Cell((int x, int y) position, TrapType trapType)
        {
            SetPosition(position);
            SetPoints(0);
            Walls = new Dictionary<Direction, bool>();
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                Walls[direction] = true;
            }
            _trap = TrapsInitialData.GetInitialTrap(trapType);
        }

        public Cell(ICell cell)
        {
            Walls = new Dictionary<Direction, bool>();
            SetPosition(cell.Position);
            SetPoints(cell.Points);
            SetWalls(cell.Walls);
            _trap = cell.Trap;
        }
        
        public void SetPosition((int x, int y) newPosition)
        {
            Position = newPosition;
        }

        public void SetPoints(int points)
        {
            Points = points;
        }

        public void SetWalls(Dictionary<Direction, bool> newWalls)
        {
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                SetWall(direction, newWalls[direction]);
            }
        }

        public void SetWall(Direction direction, bool value)
        {
            Walls[direction] = value;
        }
        public void RotateWalls(bool clockwise)
        {
            var newWalls = new Dictionary<Direction, bool>(); 
            if (clockwise)
            {
                newWalls[Direction.Up] = Walls[Direction.Left];
                newWalls[Direction.Right] = Walls[Direction.Up];
                newWalls[Direction.Down] = Walls[Direction.Right];
                newWalls[Direction.Left] = Walls[Direction.Down];
            }
            else
            {
                newWalls[Direction.Up] = Walls[Direction.Right];
                newWalls[Direction.Right] = Walls[Direction.Down];
                newWalls[Direction.Down] = Walls[Direction.Left];
                newWalls[Direction.Left] = Walls[Direction.Up];
            }

            foreach (var d in Enum.GetValues(typeof(Direction)))
            {
                SetWall((Direction)d, newWalls[(Direction)d]);
            }
        }
    }
}