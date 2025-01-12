using Core.Models.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Interfaces.Entities
{
    public interface ITokenView
    {   
        Image tokenImage { get; }
        Sprite tokenImage256 { get; }
        void Initialize(ITokenController token);
        void UpdateVisuals();
    }
}