using Core.Interface.Controllers;
using Core.Interface.Models;
using Managers;

namespace Core.Traps
{
    /// <summary>
    /// Freeze trap: Reduces movement for the current turn.
    /// </summary>
    public class FreezeEffect : ITrapEffect
    {
        private const int MovementReduction = 2;
        
        public void Activate(IPieceController piece, ITrap trap)
        {
            // Reduce remaining movements
            int newMoves = piece.PieceModel.RemainingMovs - MovementReduction;
            if (newMoves < 0) newMoves = 0;
            piece.PieceModel.SetRemainingMovs(newMoves);
            
            // Apply frozen status
            piece.PieceModel.SetCurrentStatus(StatusEffect.Frozen);
        }
    }
}
