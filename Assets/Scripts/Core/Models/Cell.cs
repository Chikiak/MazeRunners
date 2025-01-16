using System;
using System.Collections.Generic;
using Core.Interfaces;
using Core.Interfaces.Entities;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

namespace Core.Models
{
    public class Cell : ICell
    {
        #region Properties
        private (int, int) _position;
        public (int, int) Position => _position;
        
        private bool[] _walls;
        public bool[] Walls => _walls;

        private bool _selectable;
        public bool Selectable => _selectable;
        
        private List<ITokenController> _tokens;
        public List<ITokenController> Tokens => _tokens;
        
        private TrapTypes _type = TrapTypes.NoTrap;
        public TrapTypes Type => _type;
        #endregion

        #region Methods
        public Cell((int, int) position)
        {
            _position = position;
            _walls = new bool[4];
            for (int i = 0; i < _walls.Length; i++)
            { 
                _walls[i] = true;
            }
        }
        public Cell(ICell cell)
        {
            _position = cell.Position;
            _walls = cell.Walls;
            _tokens = new List<ITokenController>();
            SetSelectable(cell.Selectable);
            if (cell.Tokens == null) return;
            for (int i = 0; i < cell.Tokens.Count; i++)
            {
                AddToken(cell.Tokens[i]);
            }
        }

        public void SetType(TrapTypes type)
        {
            _type = type;
        }
        public void SetWall(Directions direction, bool value)
        {
            _walls[(int) direction] = value;
        }

        public void AddToken(ITokenController token)
        {
            _tokens.Add(token);
        }

        public void RemoveToken(ITokenController token)
        {
            _tokens.Remove(token);
        }

        public void ClearTokens()
        {
            _tokens.Clear();
        }

        public void SetSelectable(bool selectable)
        {
            _selectable = selectable;
        }

        public virtual void ApplyEffects()
        {
            return;
        }
        #endregion
    }
}