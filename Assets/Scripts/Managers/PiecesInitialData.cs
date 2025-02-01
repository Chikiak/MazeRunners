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
                PieceType.Lancer => GetNewLancer(),
                PieceType.Gladiator => GetNewGladiator(),
                PieceType.Explorer => GetNewExplorer(),
                PieceType.Thief => GetNewThief(),
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
            newModel.SetRemainingMovs(2);
            newModel.SetSpeed(2);
            newModel.SetPoints(0);
            newModel.SetAbilityCooldown(4);
            newModel.SetCurrentCooldown(4);
            newModel.SetCurrentStatus(StatusEffect.None);
            newModel.SetDamage(5);
            newModel.SetRangeType(RangeType.Square);
            newModel.SetSpecialRange(3);
            return newModel;
        }
        static IPieceModel GetNewLancer()
        {
            IPieceModel newModel = new PieceModel();
            newModel.SetPiece(PieceType.Lancer);
            newModel.SetMaxHealth(40);
            newModel.SetHealth(40);
            newModel.SetMaxSpeed(3);
            newModel.SetRemainingMovs(3);
            newModel.SetSpeed(3);
            newModel.SetPoints(0);
            newModel.SetAbilityCooldown(5);
            newModel.SetCurrentCooldown(5);
            newModel.SetCurrentStatus(StatusEffect.None);
            newModel.SetDamage(10);
            newModel.SetRangeType(RangeType.Line);
            newModel.SetSpecialRange(2);
            return newModel;
        }
        static IPieceModel GetNewDestroyer()
        {
            IPieceModel newModel = new PieceModel();
            newModel.SetPiece(PieceType.Destroyer);
            newModel.SetMaxHealth(50);
            newModel.SetHealth(50);
            newModel.SetMaxSpeed(2);
            newModel.SetRemainingMovs(2);
            newModel.SetSpeed(2);
            newModel.SetPoints(0);
            newModel.SetAbilityCooldown(3);
            newModel.SetCurrentCooldown(3);
            newModel.SetCurrentStatus(StatusEffect.None);
            newModel.SetDamage(10);
            return newModel;
        }
        static IPieceModel GetNewGladiator()
        {
            IPieceModel newModel = new PieceModel();
            newModel.SetPiece(PieceType.Gladiator);
            newModel.SetMaxHealth(45);
            newModel.SetHealth(45);
            newModel.SetMaxSpeed(3);
            newModel.SetRemainingMovs(3);
            newModel.SetSpeed(3);
            newModel.SetPoints(0);
            newModel.SetAbilityCooldown(5);
            newModel.SetCurrentCooldown(5);
            newModel.SetCurrentStatus(StatusEffect.None);
            newModel.SetDamage(8);
            return newModel;
        }
        static IPieceModel GetNewThief()
        {
            IPieceModel newModel = new PieceModel();
            newModel.SetPiece(PieceType.Thief);
            newModel.SetMaxHealth(15);
            newModel.SetHealth(15);
            newModel.SetMaxSpeed(4);
            newModel.SetRemainingMovs(4);
            newModel.SetSpeed(4);
            newModel.SetPoints(0);
            newModel.SetAbilityCooldown(3);
            newModel.SetCurrentCooldown(3);
            newModel.SetCurrentStatus(StatusEffect.None);
            newModel.SetDamage(6);
            newModel.SetRangeType(RangeType.Diamond);
            newModel.SetSpecialRange(2);
            return newModel;
        }
        static IPieceModel GetNewExplorer()
        {
            IPieceModel newModel = new PieceModel();
            newModel.SetPiece(PieceType.Explorer);
            newModel.SetMaxHealth(20);
            newModel.SetHealth(20);
            newModel.SetMaxSpeed(5);
            newModel.SetRemainingMovs(5);
            newModel.SetSpeed(5);
            newModel.SetPoints(0);
            newModel.SetAbilityCooldown(4);
            newModel.SetCurrentCooldown(4);
            newModel.SetCurrentStatus(StatusEffect.None);
            newModel.SetDamage(4);
            return newModel;
        }

    }
}