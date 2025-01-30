using Managers;

namespace Core.Interface.Models
{
    public interface ITrap
    {
        TrapType TrapType { get; }
        int AbilityCooldown { get; }
        int CurrentCooldown { get; }
        
        void SetType(TrapType trapType);
        void SetAbilityCooldown(int abilityCooldown);
        void SetCurrentCooldown(int currentCooldown);
    }
}