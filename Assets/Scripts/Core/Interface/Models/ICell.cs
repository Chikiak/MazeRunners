using System.Collections.Generic;
using Managers;

namespace Core.Interface.Models
{
    /// <summary>
    /// Interface representing a cell in the maze.
    /// </summary>
    public interface ICell
    {
        /// <summary>
        /// Gets the position of the cell in the maze grid.
        /// </summary>
        (int x, int y) Position { get; }
        
        /// <summary>
        /// Gets the points available in this cell.
        /// </summary>
        int Points { get; }
        
        /// <summary>
        /// Gets the walls of the cell for each direction.
        /// </summary>
        Dictionary<Direction, bool> Walls { get; }
        
        /// <summary>
        /// Gets the trap in this cell.
        /// </summary>
        ITrap Trap { get; }
        
        /// <summary>
        /// Gets whether this cell is currently selectable.
        /// </summary>
        bool IsSelectable { get; }

        /// <summary>
        /// Sets whether this cell is selectable.
        /// </summary>
        void SetSelectable(bool selectable);
        
        /// <summary>
        /// Sets the position of the cell.
        /// </summary>
        void SetPosition((int x, int y) newPosition);
        
        /// <summary>
        /// Sets the points in this cell.
        /// </summary>
        void SetPoints(int points);
        
        /// <summary>
        /// Sets all walls of the cell.
        /// </summary>
        void SetWalls(Dictionary<Direction, bool> newWalls);
        
        /// <summary>
        /// Sets a specific wall of the cell.
        /// </summary>
        void SetWall(Direction direction, bool value);
        
        /// <summary>
        /// Rotates the walls of the cell.
        /// </summary>
        void RotateWalls(bool clockwise);
    }
}