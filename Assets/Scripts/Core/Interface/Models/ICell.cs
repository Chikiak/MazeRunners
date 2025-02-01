using System.Collections.Generic;
using Managers;

namespace Core.Interface.Models
{
    public interface ICell
    {
        (int x, int y) Position { get; }
        int Points { get; }
        Dictionary<Direction, bool> Walls { get; }
        ITrap Trap { get; }
        bool IsSelectable { get; }


        void SetSelectable(bool selectable);
        void SetPosition((int x, int y) newPosition);
        void SetPoints(int points);
        void SetWalls(Dictionary<Direction, bool> newWalls);
        void SetWall(Direction direction, bool value);
        void RotateWalls(bool clockwise);
    }
}