using System;
using Core.Interface.Controllers;
using Core.Interface.Models;
using Managers;

namespace Core.Traps
{
    /// <summary>
    /// Spikes trap: Deals random damage to the piece.
    /// </summary>
    public class SpikesEffect : ITrapEffect
    {
        private static readonly Random Random = new Random();
        private const int MinDamage = 0;
        private const int MaxDamage = 11;
        
        public void Activate(IPieceController piece, ITrap trap)
        {
            int damage = Random.Next(MinDamage, MaxDamage);
            piece.TakeDamage(damage);
            
            if (!piece.IsAlive())
            {
                PieceManager.DefeatedPieces.Add(piece);
            }
        }
    }
}
