using System;

namespace Core.Interfaces.Entities
{
    public interface ITokenController
    {
        IToken Model { get; }
        ITokenView View { get; }
        Players PlayerID { get; }
        bool IsAlive { get; }
        
        void UseAbility();
        
        void SetView(ITokenView newView);
        void TakeDamage(int damage);
        void RestoreHealth(int amount);
        void Die();
        void Revive();
    }
}