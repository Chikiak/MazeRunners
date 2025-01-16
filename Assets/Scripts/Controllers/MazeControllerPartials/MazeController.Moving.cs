using System.Collections;
using System.Collections.Generic;
using Core.Interfaces.Entities;
using Core.Models.Traps;
using UnityEngine;

namespace Controllers
{
    public partial class MazeController
    {
        #region Moving

        private void StartMove(ITokenController token, (int, int) targetPos)
        {
            if (token == null || _isMoving) return;

            _movementPath = GetMovementPath(token.Model.Position, targetPos);
            if (_movementPath == null || _movementPath.Count == 0) return;

            GameManager.Instance.StartCoroutine(ExecuteMovement());
            OnMazeChanged();
        }

        private List<Directions> GetMovementPath((int, int) startPos, (int, int) targetPos)
        {
            var path = new List<Directions>();
            var visited = new HashSet<(int, int)>();
            var queue = new Queue<((int, int) pos, List<Directions> path)>();

            queue.Enqueue((startPos, new List<Directions>()));
            visited.Add(startPos);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                var currentPos = current.pos;
                var currentPath = current.path;

                if (currentPos == targetPos)
                {
                    return currentPath;
                }

                foreach (Directions direction in System.Enum.GetValues(typeof(Directions)))
                {
                    if (!IsPathOpen(GetMazeFace(0).Cells[currentPos.Item1, currentPos.Item2], direction))
                        continue;

                    var nextPos = GetNextPosition(currentPos, direction);
                    if (!visited.Contains(nextPos))
                    {
                        var newPath = new List<Directions>(currentPath) { direction };
                        queue.Enqueue((nextPos, newPath));
                        visited.Add(nextPos);
                    }
                }
            }

            return null;
        }

        private void GetTrapsInMovement((int,int) currentPos)
        {
            var newPath = new List<Directions>();
            
            foreach (var direction in _movementPath)
            {
                var nextPos = GetNextPosition(currentPos, direction);
                currentPos = nextPos;
                newPath.Add(direction);
                var cell = GetMazeFace(0).Cells[currentPos.Item1, currentPos.Item2];
                if (cell.Type == TrapTypes.NoTrap) continue;
                var trap = cell as Trap;
                if (trap != null && trap.AbilityIsReady)
                {
                    _movementPath = newPath;
                    return;
                }
            }
        }
        private IEnumerator ExecuteMovement()
        {
            var token = GameManager.SelectedToken;
            _isMoving = true;
            var currentPos = token.Model.Position;
            GetTrapsInMovement(currentPos);

            _maze.TokensMaze[currentPos.Item1, currentPos.Item2].Remove(token);

            foreach (var direction in _movementPath)
            {
                var nextPos = GetNextPosition(currentPos, direction);

                token.Model.SetPosition(nextPos);
                _maze.TokensMaze[nextPos.Item1, nextPos.Item2].Add(token);
                _maze.TokensMaze[currentPos.Item1, currentPos.Item2].Remove(token);
                currentPos = nextPos;
                token.Model.SetRemainingMovs(token.Model.RemainingMovs - 1);


                OnMazeChanged();
                yield return new WaitForSeconds(0.3f);
                continue;
                
            }
            _isMoving = false;
            _movementPath = null;
            GameManager.OnStateChanged?.Invoke(GameStates.SelectAction);
        }
        private (int, int) GetNextPosition((int, int) currentPos, Directions direction)
        {
            var (x, y) = currentPos;
            switch (direction)
            {
                case Directions.Up:
                    return (x, y - 1);
                case Directions.Down:
                    return (x, y + 1);
                case Directions.Left:
                    return (x - 1, y);
                case Directions.Right:
                    return (x + 1, y);
                default:
                    return currentPos;
            }
        }

        #endregion
    }
}