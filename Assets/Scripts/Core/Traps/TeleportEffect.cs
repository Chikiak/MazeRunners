using Core.Interface.Controllers;
using Core.Interface.Models;
using Managers;

namespace Core.Traps
{
    /// <summary>
    /// Teleport trap: Teleports the piece to a random initial position.
    /// </summary>
    public class TeleportEffect : ITrapEffect
    {
        public void Activate(IPieceController piece, ITrap trap)
        {
            PieceManager.RemovePiece(piece);
            var newPosition = PieceManager.RandomInitialPosition();
            PieceManager.AddPiece(piece, newPosition);
        }
    }
}
