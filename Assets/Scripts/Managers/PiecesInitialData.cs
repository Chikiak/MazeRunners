using Core.Interface.Models;
using Core.Models;

namespace Managers
{
    /// <summary>
    /// Factory class for creating piece models with initial data.
    /// </summary>
    public static class PiecesInitialData
    {
        /// <summary>
        /// Creates a new piece model with initial data for the specified piece type.
        /// </summary>
        /// <param name="pieceType">The type of piece to create.</param>
        /// <returns>The initialized piece model.</returns>
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
                PieceType.Archer => GetNewArcher(),
                PieceType.Tank => GetNewTank(),
                _ => null
            };
        }

        private static IPieceModel GetNewHealer()
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
        
        private static IPieceModel GetNewLancer()
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
        
        private static IPieceModel GetNewDestroyer()
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
            newModel.SetRangeType(RangeType.Diamond);
            newModel.SetSpecialRange(1);
            return newModel;
        }
        
        private static IPieceModel GetNewGladiator()
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
            newModel.SetRangeType(RangeType.Square);
            newModel.SetSpecialRange(1);
            return newModel;
        }
        
        private static IPieceModel GetNewThief()
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
        
        private static IPieceModel GetNewExplorer()
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
            newModel.SetRangeType(RangeType.Diamond);
            newModel.SetSpecialRange(3);
            return newModel;
        }
        
        private static IPieceModel GetNewArcher()
        {
            IPieceModel newModel = new PieceModel();
            newModel.SetPiece(PieceType.Archer);
            newModel.SetMaxHealth(25);
            newModel.SetHealth(25);
            newModel.SetMaxSpeed(2);
            newModel.SetRemainingMovs(2);
            newModel.SetSpeed(2);
            newModel.SetPoints(0);
            newModel.SetAbilityCooldown(3);
            newModel.SetCurrentCooldown(3);
            newModel.SetCurrentStatus(StatusEffect.None);
            newModel.SetDamage(12);
            newModel.SetRangeType(RangeType.Line);
            newModel.SetSpecialRange(4);
            return newModel;
        }
        
        private static IPieceModel GetNewTank()
        {
            IPieceModel newModel = new PieceModel();
            newModel.SetPiece(PieceType.Tank);
            newModel.SetMaxHealth(80);
            newModel.SetHealth(80);
            newModel.SetMaxSpeed(1);
            newModel.SetRemainingMovs(1);
            newModel.SetSpeed(1);
            newModel.SetPoints(0);
            newModel.SetAbilityCooldown(6);
            newModel.SetCurrentCooldown(6);
            newModel.SetCurrentStatus(StatusEffect.None);
            newModel.SetDamage(6);
            newModel.SetRangeType(RangeType.Square);
            newModel.SetSpecialRange(1);
            return newModel;
        }
    }
}