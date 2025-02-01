using Managers;

namespace Core.Interface.Models
{
    public interface IPieceModel
    {
        PieceType PieceType { get; }
        int Health { get; }
        int MaxHealth { get; }
        int Speed { get; }
        int MaxSpeed { get; }
        int RemainingMovs { get; }
        int Damage { get; }
        int AbilityCooldown { get; }
        int CurrentCooldown { get; }
        StatusEffect CurrentStatus { get; }
        int Points { get; }

        RangeType SpecialRangeType { get;}
        int SpecialRange { get; }
        
        void SetPiece(PieceType pieceType);
        void SetHealth(int health);
        void SetMaxHealth(int maxHealth);
        void SetSpeed(int speed);
        void SetMaxSpeed(int maxSpeed);
        void SetRemainingMovs(int remainingMovs);
        void SetDamage(int damage);
        void SetAbilityCooldown(int cooldown);
        void SetCurrentCooldown(int currentCooldown);
        void SetCurrentStatus(StatusEffect statusEffect);
        void SetPoints(int points);
        void SetRangeType(RangeType rangeType);
        void SetSpecialRange(int specialRange);
    }
}