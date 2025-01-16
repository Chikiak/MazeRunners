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
using Core.Models.Traps;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

namespace Controllers
{
    public partial class MazeController
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
            GameManager.ActivateTrap += HandleActivateTrap;
            GameManager.DeadPiece += TokenDead;
            _selectableCells = new List<ICell>();
            
            InitializeMaze();
        }
        private IMazeFace GetMazeFace(int faceIndex)
        {
            var face = _maze.GetFace(faceIndex);
            return face;
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
                    if (_maze.TokensMaze[i, j] == null) continue;
                    GetMazeFace(0).Cells[i, j].ApplyEffects();
                    var newCell = CellFactory.NewCell(GetMazeFace(0).Cells[i, j]);
                    newCell.ClearTokens();
                    foreach (var token in _maze.TokensMaze[i, j])
                    {
                        newCell.AddToken(token);
                    }
                    GetMazeFace(0).SetCell(i,j, newCell);
                }
            }
            _view.UpdateMaze(GetMazeFace(0));
        }
        private void OnNewTurn()
        {
            AllFight();
            var dT = new List<TokensNames>();
            
            foreach (var tokenC in GameManager.DeadTokens)
            {
                dT.Add(tokenC.Model.Name);
            }
            foreach (var tokenC in dT)
            {
                HandleTokenDead(_maze.GetToken(tokenC));
            }
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
            OnMazeChanged();
        }

        #region TrapsEffects

        private void HandleActivateTrap((int, int) cellPosition, TrapTypes trapType)
        {
            HandleSpikesActivation(cellPosition, trapType);
        } 
        private void HandleSpikesActivation((int, int) cellPosition, TrapTypes trapType)
        {
            if (trapType == TrapTypes.Spikes)
            {
                foreach (var tokenC in _maze.GetTokensInCell(cellPosition))
                {
                    tokenC.TakeDamage(20);
                }

                var trap = GetMazeFace(0).Cells[cellPosition.Item1, cellPosition.Item2] as Trap;
            }
            else
            {
                //Mas Trampas
            }
        }

        #endregion
    }
}