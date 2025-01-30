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
        int Damage { get; }
        int AbilityCooldown { get; }
        int CurrentCooldown { get; }
        StatusEffect CurrentStatus { get; }
        float Points { get; }
        
        void SetPiece(PieceType pieceType);
        void SetHealth(int health);
        void SetMaxHealth(int maxHealth);
        void SetSpeed(int speed);
        void SetMaxSpeed(int maxSpeed);
        void SetDamage(int damage);
        void SetAbilityCooldown(int cooldown);
        void SetCurrentCooldown(int currentCooldown);
        void SetCurrentStatus(StatusEffect statusEffect);
        void SetPoints(float points);
    }
}