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
                TrapType.Teleport => GetNewTeleport(),
                TrapType.AffectStats => GetNewAffectStats(),
                TrapType.Freeze => GetNewFreeze(),
                _ => GetEmpty()
            };
        }

        private static ITrap GetEmpty()
        {
            Trap trap = new Trap();
            trap.SetType(TrapType.Nothing);
            trap.SetAbilityCooldown(0);
            trap.SetCurrentCooldown(0);
            return trap;
        }
        
        private static ITrap GetNewSpikes()
        {
            Trap trap = new Trap();
            trap.SetType(TrapType.Spikes);
            trap.SetAbilityCooldown(3);
            trap.SetCurrentCooldown(0);
            return trap;
        }
        
        private static ITrap GetNewTeleport()
        {
            Trap trap = new Trap();
            trap.SetType(TrapType.Teleport);
            trap.SetAbilityCooldown(5);
            trap.SetCurrentCooldown(0);
            return trap;
        }
        
        private static ITrap GetNewAffectStats()
        {
            Trap trap = new Trap();
            trap.SetType(TrapType.AffectStats);
            trap.SetAbilityCooldown(4);
            trap.SetCurrentCooldown(0);
            return trap;
        }
        
        private static ITrap GetNewFreeze()
        {
            Trap trap = new Trap();
            trap.SetType(TrapType.Freeze);
            trap.SetAbilityCooldown(4);
            trap.SetCurrentCooldown(0);
            return trap;
        }
    }
}