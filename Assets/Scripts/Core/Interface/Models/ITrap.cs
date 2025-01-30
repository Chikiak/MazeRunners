using Managers;

namespace Core.Interface.Models
{
    public interface ITrap
    {
        TrapType TrapType { get; }
        int AbilityCooldown { get; }
        int CurrentCooldown { get; }
    }
}