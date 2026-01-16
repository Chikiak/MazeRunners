using Managers;

namespace Core.Interface.Models
{
    /// <summary>
    /// Interface representing a trap in a cell.
    /// </summary>
    public interface ITrap
    {
        /// <summary>
        /// Gets the type of trap.
        /// </summary>
        TrapType TrapType { get; }
        
        /// <summary>
        /// Gets the cooldown between trap activations.
        /// </summary>
        int AbilityCooldown { get; }
        
        /// <summary>
        /// Gets the current cooldown remaining.
        /// </summary>
        int CurrentCooldown { get; }
        
        /// <summary>
        /// Sets the trap type.
        /// </summary>
        void SetType(TrapType trapType);
        
        /// <summary>
        /// Sets the ability cooldown.
        /// </summary>
        void SetAbilityCooldown(int abilityCooldown);
        
        /// <summary>
        /// Sets the current cooldown.
        /// </summary>
        void SetCurrentCooldown(int currentCooldown);
    }
}