using System;
using Core.Interface.Models;
using Core.Models;

namespace Managers
{
    public static class TrapsInitialData
    {
        public static ITrap GetInitialTrap(TrapType trapType)
        {
            return trapType switch
            {
                TrapType.Spikes => GetNewSpikes(),
                _ => null
            };
        }

        static ITrap GetNewSpikes()
        {
            Trap trap = new Trap();
            trap.SetType(TrapType.Spikes);
            trap.SetAbilityCooldown(5);
            trap.SetCurrentCooldown(5);
            return trap;
        }
    }
}