using System;
using Core.Interface.Controllers;
using Core.Interface.Models;
using Managers;

namespace Core.Traps
{
    /// <summary>
    /// AffectStats trap: Reduces speed and deals minor damage.
    /// </summary>
    public class AffectStatsEffect : ITrapEffect
    {
        private static readonly Random Random = new Random();
        private const int MinSpeedReduction = 50; // Percentage
        private const int MaxSpeedReduction = 100; // Percentage  
        private const int MinDamage = 0;
        private const int MaxDamage = 6;
        
        public void Activate(IPieceController piece, ITrap trap)
        {
            // Fixed: Use proper floating point division for speed reduction
            int speedReductionPercent = Random.Next(MinSpeedReduction, MaxSpeedReduction);
            int newSpeed = piece.PieceModel.Speed * speedReductionPercent / 100;
            piece.PieceModel.SetSpeed(newSpeed);
            
            // Apply damage
            int damage = Random.Next(MinDamage, MaxDamage);
            piece.TakeDamage(damage);
            
            // Apply poisoned status
            piece.PieceModel.SetCurrentStatus(StatusEffect.Poisoned);
            
            if (!piece.IsAlive())
            {
                PieceManager.DefeatedPieces.Add(piece);
            }
        }
    }
}
