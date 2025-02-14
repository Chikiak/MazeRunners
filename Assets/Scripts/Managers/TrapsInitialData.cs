﻿using System;
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
                _ => GetEmpty()
            };
        }

        static ITrap GetEmpty()
        {
            Trap trap = new Trap();
            trap.SetType(TrapType.Nothing);
            trap.SetAbilityCooldown(0);
            trap.SetCurrentCooldown(0);
            return trap;
        }
        static ITrap GetNewSpikes()
        {
            Trap trap = new Trap();
            trap.SetType(TrapType.Spikes);
            trap.SetAbilityCooldown(3);
            trap.SetCurrentCooldown(3);
            return trap;
        }
    }
}