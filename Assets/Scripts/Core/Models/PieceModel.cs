using Core.Interface.Models;
using Managers;

namespace Core.Models
{
    public class PieceModel : IPieceModel
    {
        public PieceType PieceType { get; private set; }
        public int Health { get; private set; }
        public int MaxHealth { get; private set; }
        public int Speed { get; private set; }
        public int MaxSpeed { get; private set; }
        public int RemainingMovs { get; private set; }
        public int Damage { get; private set; }
        public int AbilityCooldown { get; private set; }
        public int CurrentCooldown { get; private set; }
        public StatusEffect CurrentStatus { get; private set; }
        public int Points { get; private set; }
        public RangeType SpecialRangeType { get; private set; }
        public int SpecialRange { get; private set; }

        public void SetPiece(PieceType pieceType)
        {
            PieceType = pieceType;
        }

        public void SetHealth(int health)
        {
            Health = health;
            if (Health > MaxHealth) Health = MaxHealth;
            if (Health < 0) Health = 0;
        }

        public void SetMaxHealth(int maxHealth)
        {
            MaxHealth = maxHealth;
            if (Health > MaxHealth) Health = MaxHealth;
            if (MaxHealth <= 0) MaxHealth = 1;
        }

        public void SetSpeed(int speed)
        {
            Speed = speed;
            if (Speed > MaxSpeed) Speed = MaxSpeed;
            if (Speed < 0) Speed = 0;
        }

        public void SetMaxSpeed(int maxSpeed)
        {
            MaxSpeed = maxSpeed;
            if (Speed > MaxSpeed) Speed = MaxSpeed;
            if (MaxSpeed <= 0) MaxSpeed = 1;
        }

        public void SetRemainingMovs(int remainingMovs)
        {
            RemainingMovs = remainingMovs;
        }
        public void SetDamage(int damage)
        {
            Damage = damage;
            if (Damage < 0) Damage = 0;
        }

        public void SetAbilityCooldown(int cooldown)
        {
            AbilityCooldown = cooldown;
            if (AbilityCooldown < 0) AbilityCooldown = 0;
        }

        public void SetCurrentCooldown(int currentCooldown)
        {
            CurrentCooldown = currentCooldown;
            if (CurrentCooldown < 0) CurrentCooldown = 0;
            if (CurrentCooldown > AbilityCooldown) CurrentCooldown = AbilityCooldown;
        }

        public void SetCurrentStatus(StatusEffect statusEffect)
        {
            CurrentStatus = statusEffect;
        }

        public void SetPoints(int points)
        {
            Points = points;
        }

        public void SetRangeType(RangeType rangeType)
        {
            SpecialRangeType = rangeType;
        }

        public void SetSpecialRange(int specialRange)
        {
            SpecialRange = specialRange;
        }
        
    }
}