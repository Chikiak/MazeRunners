using Core.Interface.Controllers;
using Core.Interface.Models;
using Core.Traps;
using Managers;

namespace Core.Controllers
{
    public partial class CubeController
    {
        #region OnTrap

        public static void ActivateTrap(IPieceController piece, ITrap trap)
        {
            if (trap.CurrentCooldown > 0) return;
            
            var effect = TrapEffectFactory.CreateEffect(trap.TrapType);
            effect?.Activate(piece, trap);
            
            trap.SetCurrentCooldown(trap.AbilityCooldown);
        }

        #endregion
    }
}