using Core.Interface.Models;
using Core.Models;

namespace Managers
{
    public static class PiecesInitialData
    {
        public static IPieceModel GetInitialPiece(PieceType pieceType)
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
            IPieceModel newModel = new PieceModel();
            newModel.SetPiece(PieceType.Healer);
            newModel.SetMaxHealth(30);
            newModel.SetHealth(30);
            newModel.SetMaxSpeed(2);
            newModel.SetSpeed(2);
            newModel.SetPoints(0);
            newModel.SetAbilityCooldown(4);
            newModel.SetCurrentCooldown(4);
            newModel.SetCurrentStatus(StatusEffect.None);
            newModel.SetDamage(5);
            return newModel;
        }

        static IPieceModel GetNewDestroyer()
        {
            IPieceModel newModel = new PieceModel();
            newModel.SetPiece(PieceType.Destroyer);
            newModel.SetMaxHealth(30);
            newModel.SetHealth(30);
            newModel.SetMaxSpeed(2);
            newModel.SetSpeed(2);
            newModel.SetPoints(0);
            newModel.SetAbilityCooldown(4);
            newModel.SetCurrentCooldown(4);
            newModel.SetCurrentStatus(StatusEffect.None);
            newModel.SetDamage(5);
            return newModel;
        }
    }
}