using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using Core.Interfaces;
using Core.Interfaces.Entities;
using Core.Models;
using Core.Models.Entities;
using Core.Models.Entities.SO;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

namespace Controllers
{
    public class MazeController
    {
        private readonly IMaze _maze;
        private readonly IMazeGenerator _generator;
        private readonly IMazeView _view;
        private readonly SOsManager _SOsManager;
        private List<ICell> _selectableCells;
        private List<Directions> _movementPath;
        private bool _isMoving;
        
        public Action<(int,int)> OnCellSelected;

        public MazeController(SOsManager sosManager, IMaze maze, IMazeGenerator generator, IMazeView view)
        {
            _maze = maze;
            _generator = generator;
            _view = view;
            _view.OnCellClicked += HandleSelectedCell;
            _SOsManager = sosManager;
            GameManager.SelectingCell += SetSelectableCells;
            GameManager.AbilityUsed += HandleAblityUsed;
            GameManager.NewTurn += OnNewTurn;
            GameManager.MovePiece += StartMove;
            _selectableCells = new List<ICell>();
            
            InitializeMaze();
        }
        private IMazeFace GetMazeFace(int faceIndex)
        {
            return _maze.GetFace(faceIndex);
        }
        private void InitializeMaze()
        {
            _view.InitializeMaze(_maze.Size);
            GenerateMaze();
            GameManager.OnMazeChanged += OnMazeChanged;
        }
        public void GenerateMaze()
        {
            for (int i = 0; i < _maze.NumberOfFaces; i++)
            {
                var face = _generator.GenerateFace();
                _maze.SetFace(i, face);
            }
        }
        private void OnMazeChanged()
        {
            
            for (int i = 0; i < _maze.Size; i++)
            {
                for (int j = 0; j < _maze.Size; j++)
                {
                    if (_maze.TokensMaze[i, j] != null)
                    {
                        var newCell = new Cell(GetMazeFace(0).Cells[i, j]);
                        newCell.ClearTokens();
                        foreach (var token in _maze.TokensMaze[i, j])
                        {
                            newCell.AddToken(token);
                        }
                        GetMazeFace(0).SetCell(i,j, newCell);
                    }
                }
            }
            _view.UpdateMaze(GetMazeFace(0));
            if (GameManager.GameState == GameStates.PieceOnBoardSelection)
            {
                GameManager.NewTurn?.Invoke();
            }
        }
        private void OnNewTurn()
        {
            AllFight();
            foreach (var list in _maze.TokensMaze)
            {
                foreach (var token in list)
                {
                    if (token.PlayerID == GameManager.Turn)
                    {
                        token.Model.SetCooldown(token.Model.CurrentCooldown - 1);
                        token.Model.SetRemainingMovs(token.Model.Speed);
                    }
                }
            }
        }
        private void AllFight()
        {
            var tlist = _maze.TokensMaze;
            foreach (var list in tlist)
            {
                if (list.Count == 2)
                {
                    Fight(list[0], list[1]);
                }
            }
        }
        private void Fight(ITokenController token1, ITokenController token2)
        {
            token1.TakeDamage(token2.Model.Dmg);
            if (token1.Model.Health <= 0)
            {
                TokenDead(token1);
                _maze.TokensMaze[token1.Model.Position.Item1, token1.Model.Position.Item2].Remove(token1);
            }
            else
            {
                token2.TakeDamage(token1.Model.Dmg);
                if (token2.Model.Health <= 0)
                {
                    TokenDead(token2);
                    _maze.TokensMaze[token2.Model.Position.Item1, token2.Model.Position.Item2].Remove(token2);
                }
            }
        }
        private void TokenDead(ITokenController token)
        {
            token.Model.SetHealth(token.Model.MaxHealth);
            Random r = new Random();
            int pos1 = r.Next(0, _maze.Size);
            int pos2 = r.Next(0, _maze.Size);
            token.Model.SetPosition((pos1, pos2));
            _maze.TokensMaze[token.Model.Position.Item1, token.Model.Position.Item2].Add(token);
        }
        
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
                facesI = clockwise ? new []{ 0, 1, 5, 3} : new []{ 0, 3, 5, 1};
            }
            else
            {
                facesI = clockwise ? new []{ 0, 4, 5, 2} : new []{ 0, 2, 5, 4};
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
                        var newCell = new Cell(GetMazeFace(facesI[i]).Cells[j, rotateIndex]);
                        tempCells[i][j] = newCell;
                    }
                }
                
                for (int i = 0; i < facesI.Length - 1; i++)
                {
                    for (int j = 0; j < _maze.Size; j++)
                    {
                        var face = GetMazeFace(facesI[i]);
                        var newCell = tempCells[i + 1][j];
                        face.SetCell(j, rotateIndex, new Cell(newCell));
                    }
                }
                
                for (int i = 0; i < _maze.Size; i++)
                {
                    GetMazeFace(facesI[facesI.Length - 1]).SetCell(i, rotateIndex, new Cell(tempCells[0][i]));
                }
            }
            else
            {
                for (int i = 0; i < facesI.Length; i++)
                {
                    for (int j = 0; j < _maze.Size; j++)
                    {
                        var newCell = new Cell(GetMazeFace(facesI[i]).Cells[rotateIndex, j]);
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
        private void RotateExtremeFace(bool horizontal, int rotateIndex,bool clockwise)
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
    
            var face = GetMazeFace(faceToRotate);
            RotateCellsInFace(clockwise, face);
        }
        private void RotateCellsInFace(bool clockwise, IMazeFace face)
        {
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
            
            
            face.SetCells(temp);
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
                cell.SetWall((Directions) d, newWalls[(int)d]);
            }
        }
        #endregion

        #region Abilities

        void HandleAblityUsed(ITokenController tokenController)
        {
            if (tokenController == null) return;
            
            if (!tokenController.Model.AbilityIsReady)
            {
                Debug.Log($"Ability isnt ready, ready in {tokenController.Model.CurrentCooldown} turns");
                return;
            }
            HealerAbility(tokenController);
        }
        void HealerAbility(ITokenController tokenController)
        {
            if (tokenController.Model.Name is TokensNames.Healer)
            {
                var model = tokenController.Model;
                List<ICell> cells = GetCellsInRange(model.SpecialRangeType, model.SpecialRange, model.Position);
                
                foreach (var cell in cells)
                {
                    if (cell.Tokens != null)
                    {
                        var tokenList = _maze.TokensMaze[cell.Position.Item1, cell.Position.Item2];
                        cell.ClearTokens();
                        foreach (var token in tokenList)
                        {
                            token.RestoreHealth(20);
                            cell.AddToken(token);
                        }
                    }
                }
                tokenController.Model.SetAbilityIsReady(false);

            }
            else
            {
                DestroyerAbility(tokenController);
            }
        }
        void DestroyerAbility(ITokenController tokenController)
        {
            if (tokenController.Model.Name is TokensNames.Destroyer)
            {
                var model = tokenController.Model;
                List<ICell> cells = GetCellsInRange(model.SpecialRangeType, model.SpecialRange, model.Position);
                
                foreach (var cell in cells)
                {
                    foreach (var d in Enum.GetValues(typeof(Directions)))
                    {
                        cell.SetWall((Directions) d, false);
                    }

                    if (cell.Position.Item1 == 0)
                    {
                        cell.SetWall(Directions.Left, true);
                    }
                    else if (cell.Position.Item1 == _maze.Size - 1)
                    {
                        cell.SetWall( Directions.Right, true);
                    }
                    if (cell.Position.Item2 == 0)
                    {
                        cell.SetWall( Directions.Up, true);
                    }
                    else if (cell.Position.Item2 == _maze.Size - 1)
                    {
                        cell.SetWall(Directions.Down, true);
                    }
                }
                tokenController.Model.SetAbilityIsReady(false);
                OnMazeChanged();
            }
        }

        #endregion
        
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
        private List<ICell> GetCellsInSquareRange(int size, (int,int) pos)
        {
            if (size%2 == 0) throw new ArgumentException("Size cant be divisible by 2");
            List<ICell> cells = new List<ICell>();
            
            int iniX = Math.Max(0, pos.Item1 - (size - 1) / 2);
            int iniY = Math.Max(0, pos.Item2 - (size - 1) / 2);
            int finX = Math.Min(_maze.Size - 1, pos.Item1 + (size - 1) / 2);
            int finY = Math.Min(_maze.Size - 1, pos.Item2 + (size - 1) / 2);
            
            for (int x = iniX; x <= finX; x++)
            {
                for (int y = iniY; y <= finY; y++)
                {
                    cells.Add(GetMazeFace(0).Cells[x,y]);
                }
            }
            return cells;
        }
        /*private List<ICell> GetCellsInPathRange(int range, (int,int) pos)
        {
            List<ICell> cells = new List<ICell>();
            var mazeFace = GetMazeFace(0);
            //ToDo
            
            
            return cells;
        }*/
        private List<ICell> GetCellsInPathRange(int range, (int,int) pos) 
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

        #region Selecting
        public void PutInitialPiece()
        {
            var tokenController = GameManager.SelectedToken;
            if (tokenController == null) return;
            
            var cell = GetMazeFace(0).Cells[GameManager.SelectedCell.Item1, GameManager.SelectedCell.Item2];
            if (cell == null) return;
            cell.AddToken(tokenController);
            tokenController.Model.SetPosition((GameManager.SelectedCell.Item1, GameManager.SelectedCell.Item2));
            _maze.TokensMaze[GameManager.SelectedCell.Item1, GameManager.SelectedCell.Item2].Add(tokenController);
            _view.UpdateMaze(GetMazeFace(0));
        }
        private void SetSelectableCells(RangesType rangeType, int size, (int,int) pos)
        {
            int center = (_maze.Size - 1) / 2;
            
            var cells = GetCellsInRange(rangeType, size, pos);
            if (GameManager.GameState == GameStates.PutingInitialPiece)
                cells.Remove(GetMazeFace(0).Cells[center, center]);

            foreach (var cell in cells)
            {
                if (GameManager.GameState == GameStates.PutingInitialPiece && cell.Tokens.Count > 0) continue;
                
                _selectableCells.Add(cell);
                cell.SetSelectable(true);
                (int,int) thisPos = cell.Position;
                GetMazeFace(0).SetCell(thisPos.Item1, thisPos.Item2, cell);
            }
            _view.UpdateMaze(GetMazeFace(0));
        }
        private void SetSelectableCells()
        {
            if (GameManager.GameState == GameStates.PutingInitialPiece)
            {
                SetSelectableCells(RangesType.Square, 3, ((_maze.Size - 1) / 2, (_maze.Size - 1) / 2));
            }
            else if (GameManager.GameState == GameStates.CellSelection)
            {
                SetSelectableCells(RangesType.Path, GameManager.SelectedToken.Model.RemainingMovs, GameManager.SelectedToken.Model.Position);
            }
        }
        private void HandleSelectedCell((int,int) position)
        {
            foreach (var cell in _selectableCells)
            {
                cell.SetSelectable(false);
                GetMazeFace(0).SetCell(cell.Position.Item1, cell.Position.Item2, cell);
            }
            _view.UpdateMaze(GetMazeFace(0));
            OnCellSelected?.Invoke(position);
        }
        public IToken GetToken(TokensNames tokenName)
        {
            foreach (var tokenList in _maze.TokensMaze)
            {
                foreach (var tokenController in tokenList)
                {
                    if (tokenController.Model.Name == tokenName)
                    {
                        return tokenController.Model;
                    }
                }
            }
            throw new Exception($"Token not found: {tokenName}");
        }
        #endregion

        #region Moving
        private void StartMove(ITokenController token, (int, int) targetPos)
        {
            if (token == null || _isMoving) return;
        
            _movementPath = GetMovementPath(token.Model.Position, targetPos);
            if (_movementPath == null || _movementPath.Count == 0) return;

            GameManager.Instance.StartCoroutine(ExecuteMovement(token, targetPos));
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
        private IEnumerator ExecuteMovement(ITokenController token, (int, int) targetPos)
        {
            _isMoving = true;
            var currentPos = token.Model.Position;

            _maze.TokensMaze[currentPos.Item1, currentPos.Item2].Remove(token);

            foreach (var direction in _movementPath)
            {
                var nextPos = GetNextPosition(currentPos, direction);
                
                _maze.TokensMaze[nextPos.Item1, nextPos.Item2].Add(token);
                _maze.TokensMaze[currentPos.Item1, currentPos.Item2].Remove(token);
                token.Model.SetPosition(nextPos);
                currentPos = nextPos;
                token.Model.SetRemainingMovs(token.Model.RemainingMovs - 1);

                OnMazeChanged();

                yield return new WaitForSeconds(0.3f);
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