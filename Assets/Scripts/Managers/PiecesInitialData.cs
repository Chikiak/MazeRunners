using Core.Interface.Models;

namespace Managers
{
    public static class PiecesInitialData
    {
        static IPieceModel GetInitialPiece(PieceType pieceType)
        {
            return pieceType switch
            {
                PieceType.Healer => GetNewHealer(),
                PieceType.Destroyer => GetNewDestroyer(),
                _ => null
            };
        }

        static IPieceModel GetNewHealer()
        {
            throw new System.NotImplementedException();
        }

        static IPieceModel GetNewDestroyer()
        {
            throw new System.NotImplementedException();
        }
    }
}