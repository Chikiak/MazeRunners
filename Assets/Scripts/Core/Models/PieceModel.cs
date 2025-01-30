using Core.Interface.Models;
using Managers;

namespace Core.Models
{
    public class PieceModel : IPieceModel
    {
        private PieceType _pieceType;
        public PieceType PieceType => _pieceType;
        private int _health;
        public int Health => _health;
        private int _maxHealth;
        public int MaxHealth => _maxHealth;
        private int _speed;
        public int Speed => _speed;
        private int _maxSpeed;
        public int MaxSpeed => _maxSpeed;
        private int _damage;
        public int Damage => _damage;
        private int _abilityCooldown;
        public int AbilityCooldown => _abilityCooldown;
        private int _currentCooldown;
        public int CurrentCooldown => _currentCooldown;
        private StatusEffect _currentStatus;
        public StatusEffect CurrentStatus => _currentStatus;
        private float _points;
        public float Points => _points;
        
        public void SetPiece(PieceType pieceType)
        {
            _pieceType = pieceType;
        }

        public void SetHealth(int health)
        {
            _health = health;
            if (Health > MaxHealth) _health = MaxHealth;
            if (Health < 0) _health = 0;
        }

        public void SetMaxHealth(int maxHealth)
        {
            _maxHealth = maxHealth;
            if (Health > MaxHealth) _health = MaxHealth;
            if (MaxHealth <= 0) _maxHealth = 1;
        }

        public void SetSpeed(int speed)
        {
            _speed = speed;
            if (Speed > MaxSpeed) _speed = MaxSpeed;
            if (Speed < 0) _speed = 0;
        }

        public void SetMaxSpeed(int maxSpeed)
        {
            _maxSpeed = maxSpeed;
            if (Speed > MaxSpeed) _speed = MaxSpeed;
            if (MaxSpeed <= 0) _maxSpeed = 1;
        }

        public void SetDamage(int damage)
        {
            _damage = damage;
            if (Damage < 0) _damage = 0;
        }

        public void SetAbilityCooldown(int cooldown)
        {
            _abilityCooldown = cooldown;
            if (AbilityCooldown < 0) _abilityCooldown = 0;
        }

        public void SetCurrentCooldown(int currentCooldown)
        {
            _currentCooldown = currentCooldown;
            if (CurrentCooldown < 0) _currentCooldown = 0;
            if (CurrentCooldown > AbilityCooldown) _currentCooldown = AbilityCooldown;
        }

        public void SetCurrentStatus(StatusEffect statusEffect)
        {
            _currentStatus = statusEffect;
        }

        public void SetPoints(float points)
        {
            _points = points;
        }
    }
}