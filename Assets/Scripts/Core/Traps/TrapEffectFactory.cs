using System;
using System.Collections.Generic;
using Managers;

namespace Core.Traps
{
    /// <summary>
    /// Factory class for creating trap effects using Factory Pattern.
    /// </summary>
    public static class TrapEffectFactory
    {
        private static readonly Dictionary<TrapType, Func<ITrapEffect>> EffectCreators;
        
        static TrapEffectFactory()
        {
            EffectCreators = new Dictionary<TrapType, Func<ITrapEffect>>
            {
                { TrapType.Spikes, () => new SpikesEffect() },
                { TrapType.Teleport, () => new TeleportEffect() },
                { TrapType.AffectStats, () => new AffectStatsEffect() },
                { TrapType.Freeze, () => new FreezeEffect() }
            };
        }
        
        /// <summary>
        /// Creates a trap effect for the specified trap type.
        /// </summary>
        /// <param name="trapType">The type of trap.</param>
        /// <returns>The trap effect instance, or null if no effect is defined.</returns>
        public static ITrapEffect CreateEffect(TrapType trapType)
        {
            if (EffectCreators.TryGetValue(trapType, out var creator))
            {
                return creator();
            }
            
            return null; // No effect for TrapType.Nothing
        }
    }
}
