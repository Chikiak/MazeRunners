using System;
using System.Collections.Generic;
using Core.Interface.Models;
using Managers;

namespace Core.Models
{
    public class Cell : ICell
    {
        private (int x, int y) _position;
        public (int x, int y) Position => _position;
        private int _points;
        public int Points => _points;
        private Dictionary<Direction, bool> _walls;
        public Dictionary<Direction, bool> Walls => _walls;
        private ITrap _trap;
        public ITrap Trap => _trap;

        public Cell((int x, int y) position, TrapType trapType)
        {
            SetPosition(position);
            SetPoints(0);
            _walls = new Dictionary<Direction, bool>();
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                _walls[direction] = true;
            }
            _trap = TrapsInitialData.GetInitialTrap(trapType);
        }

        public Cell(ICell cell)
        {
            SetPosition(cell.Position);
            SetPoints(cell.Points);
            SetWalls(cell.Walls);
            _trap = cell.Trap;
        }
        
        public void SetPosition((int x, int y) newPosition)
        {
            _position = newPosition;
        }

        public void SetPoints(int points)
        {
            _points = points;
        }

        public void SetWalls(Dictionary<Direction, bool> newWalls)
        {
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                _walls[direction] = newWalls[direction];
            }
        }

        public void SetWall(Direction direction, bool value)
        {
            _walls[direction] = value;
        }
    }
}