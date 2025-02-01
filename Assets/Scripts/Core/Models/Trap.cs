using Core.Interface.Models;
using Managers;

namespace Core.Models
{
    public class Trap : ITrap
    {
        private TrapType _trapType;
        public TrapType TrapType => _trapType;
        private int _abilityCooldown;
        public int AbilityCooldown => _abilityCooldown;
        private int _currentCooldown;
        public int CurrentCooldown => _currentCooldown;
        
        public void SetType(TrapType trapType)
        {
            _trapType = trapType;
        }

        public void SetAbilityCooldown(int abilityCooldown)
        {
            _abilityCooldown = abilityCooldown;
            if (AbilityCooldown < 0 ) _abilityCooldown = 0;
        }

        public void SetCurrentCooldown(int currentCooldown)
        {
            _currentCooldown = currentCooldown;
            if (CurrentCooldown < 0 ) _currentCooldown = 0;
            if (CurrentCooldown > _abilityCooldown) _currentCooldown = AbilityCooldown;
            
        }
    }
}