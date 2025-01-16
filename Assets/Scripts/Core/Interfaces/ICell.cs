using System;
using System.Collections.Generic;
using Core.Interfaces.Entities;
using UnityEditor.U2D.Aseprite;

namespace Core.Interfaces
{
    public interface ICell
    {
        (int, int) Position { get; }
        bool[] Walls { get; }
        bool Selectable { get; }
        TrapTypes Type { get; }

        List<ITokenController> Tokens { get; }

        void ApplyEffects();
        
        void SetType(TrapTypes type);
        void SetSelectable(bool selectable);
        void SetWall(Directions direction, bool value);
        void AddToken(ITokenController token);
        void RemoveToken(ITokenController token);
        
        void ClearTokens();
        
    }
}