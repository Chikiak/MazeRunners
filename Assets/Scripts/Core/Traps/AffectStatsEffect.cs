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
        private const int MinSpeedReduction = 20; // Percentage reduction (20-50%)
        private const int MaxSpeedReduction = 50; // Percentage reduction  
        private const int MinDamage = 0;
        private const int MaxDamage = 6;
        
        public void Activate(IPieceController piece, ITrap trap)
        {
            // Calculate speed reduction (reduce speed by the percentage)
            int speedReductionPercent = Random.Next(MinSpeedReduction, MaxSpeedReduction + 1);
            int newSpeed = piece.PieceModel.Speed * (100 - speedReductionPercent) / 100;
            if (newSpeed < 1) newSpeed = 1; // Minimum speed of 1
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
