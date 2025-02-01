using System;
using Core.Interface.Controllers;
using Core.Interface.Models;
using Managers;

namespace Core.Controllers
{
    public partial class CubeController
    {
        #region OnTrap

        private static Random random = new Random();

        public static void ActivateTrap(IPieceController piece, ITrap trap)
        {
            if (trap.CurrentCooldown > 0) return;
            switch (trap.TrapType)
            {
                case TrapType.Spikes:
                    ActivateSpikes(piece, trap);
                    break;
                case TrapType.Teleport:
                    ActivateTeleport(piece, trap);
                    break;
                case TrapType.AffectStats:
                    ActivateAffectStats(piece, trap);
                    break;
            }
            trap.SetCurrentCooldown(trap.AbilityCooldown);
        }

        private static void ActivateSpikes(IPieceController piece, ITrap trap)
        {
            piece.TakeDamage(_random.Next(0, 11));
            if (!piece.IsAlive())
            {
                PieceManager.DefeatedPieces.Add(PieceManager.PiecesMatrix[piece.Position.x, piece.Position.y][1]);
            }
        }

        private static void ActivateTeleport(IPieceController piece, ITrap trap)
        {
            PieceManager.RemovePiece(piece);
            PieceManager.AddPiece(piece, PieceManager.RandomInitialPosition());
        }

        private static void ActivateAffectStats(IPieceController piece, ITrap trap)
        {
            piece.PieceModel.SetSpeed(piece.PieceModel.Speed * (random.Next(5, 10) / 10));
            piece.TakeDamage(random.Next(0, 6));
            if (!piece.IsAlive())
            {
                PieceManager.DefeatedPieces.Add(PieceManager.PiecesMatrix[piece.Position.x,piece.Position.y][1]);
            }
        }

        #endregion
    }
}