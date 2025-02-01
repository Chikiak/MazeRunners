using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Core.Interface.Models;
using Managers;
using UnityEngine;

namespace Core.Controllers
{
    public partial class CubeController
    {
        #region Movement

        public IEnumerator StartMov()
        {
            var piece = PieceManager.SelectedPiece;
            List<(int x, int y)> cells = new List<(int x, int y)>();
            cells.Add(PieceManager.SelectedPiece.Position);
            var path = _directionsToCells[_directionIndex];
            foreach (var direction in path)
            {
                PieceManager.MovePiece(piece, direction);
                cells.Add(PieceManager.SelectedPiece.Position);
                OnCellsChanged?.Invoke(cells);
                yield return new WaitForSeconds(0.3f);
            }
        }

        private void SetSelectableCells(RangeType rangeType, int size, (int, int) pos)
        {
            int center = (_size - 1) / 2;

            var cells = GetCellsInRange(rangeType, size, pos);

            foreach (var cell in cells)
            {
                if (GameManager.GameState == GameStates.PutingInitialPiece && PieceManager.GetPiecesInCell(cell).Count > 0) continue;

                _selectableCells.Add(cell);
                Model.Cells[0][cell.x, cell.y].SetSelectable(true);
            }

            if (_selectableCells is null || _selectableCells.Count == 0)
            {
                GameManager.OnStateChanged?.Invoke(GameStates.PieceOnBoardSelection);
            }

            OnCellsChanged?.Invoke(_selectableCells);
        }
        private void SetSelectableCells()
        {
            _selectableCells = new List<(int x, int y)>();
            var piece = PieceManager.SelectedPiece;
            if (GameManager.GameState == GameStates.PutingInitialPiece)
            {
                SetSelectableCells(RangeType.Square, 3, (_size/2, _size/2));
                return;
            }
            GameManager.OnStateChanged?.Invoke(GameStates.CellSelection);
            SetSelectableCells(RangeType.Path, piece.PieceModel.RemainingMovs, piece.Position);
        }
        private List<(int x, int y)> GetCellsInRange(RangeType rangeType, int distance, (int, int) pos)
        {
            if (rangeType == RangeType.Path)
                return GetReachableCells(pos, PieceManager.SelectedPiece.PieceModel.RemainingMovs);
            return GetCellsManager.GetReachablePositions(rangeType, distance, pos, _size);
        }
        private List<(int x, int y)> GetReachableCells((int x, int y) position, int speed)
        {
            var result = new List<(int x, int y)>();
            var paths = new List<List<Direction>>();
            List<(int x, int y)> visited = new List<(int x, int y)>();
            visited.Add((position.x, position.y));
            Queue<((int x, int y) pos, List<Direction> path)> queue = new Queue<((int x, int y), List<Direction>)>();
            queue.Enqueue((position, new List<Direction>()));
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                var currentPos = current.pos;
                var currentPath = current.path;
                if (currentPath.Count == speed) continue;
                ICell cell = Model.Cells[0][currentPos.x, currentPos.y];

                foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                {
                    if (IsPathOpen(cell, direction))
                    {
                        var newPosX = cell.Position.x;
                        if (direction == Direction.Left) newPosX--;
                        else if (direction == Direction.Right) newPosX++;

                        var newPosY = cell.Position.y;
                        if (direction == Direction.Up) newPosY--;
                        else if (direction == Direction.Down) newPosY++;

                        if (PieceManager.GetPiecesInCell((newPosX,newPosY)).Count > 1) continue;
                        if (visited.Contains((newPosX, newPosY))) continue;

                        var newPath = new List<Direction> { };
                        newPath.AddRange(currentPath);
                        newPath.Add(direction);
                        queue.Enqueue(((newPosX, newPosY), newPath));
                        visited.Add((newPosX, newPosY));
                        result.Add((newPosX, newPosY));
                        paths.Add(newPath);
                    }
                }
            }

            _directionsToCells = paths;
            return result;
        }
        private bool IsPathOpen(ICell cell, Direction direction)
        {
            var mazeFace = Model.Cells[0];
            if (cell.Walls[direction]) return false;

            var newPosX = cell.Position.x;
            if (direction == Direction.Left) newPosX--;
            else if (direction == Direction.Right) newPosX++;

            var newPosY = cell.Position.y;
            if (direction == Direction.Up) newPosY--;
            else if (direction == Direction.Down) newPosY++;

            Direction adjDirection = direction switch
            {
                Direction.Up => Direction.Down,
                Direction.Down => Direction.Up,
                Direction.Left => Direction.Right,
                Direction.Right => Direction.Left,
                _ => throw new Exception("Invalid direction")
            };

            var adjCell = mazeFace[newPosX, newPosY];
            if (adjCell.Walls[adjDirection]) return false;

            return true;
        }

        #endregion
    }
}