using Core.Interfaces;
using Core.Interfaces.Entities;

namespace Core.Models
{
    public abstract class Trap : Cell, IHasAbility
    {
        #region Properties
        private bool _abilityIsReady;
        public bool AbilityIsReady => _abilityIsReady;
        private int _currentCooldown;
        public int CurrentCooldown => _currentCooldown;
        private int _abilityCooldown;
        public int AbilityCooldown => _abilityCooldown;
        
        private RangesType _specialRangeType;
        public RangesType SpecialRangeType => _specialRangeType;
        
        private int _specialRange;
        public int SpecialRange => _specialRange;

        protected Trap((int, int) position) : base(position) {}
        protected Trap(ICell cell) : base(cell) {}
        private string _abilityDescription;
        public string AbilityDescription => _abilityDescription;
        #endregion
        
        #region Setters
        public void SetCooldown(int newCooldown)
        {
            _currentCooldown = newCooldown;
            if (_currentCooldown > AbilityCooldown) _currentCooldown = AbilityCooldown;
        }
        public void SetAbilityCooldown(int cooldown)
        {
            _abilityCooldown = cooldown;
        }
        public void SetAbilityIsReady(bool isReady)
        {
            _abilityIsReady = isReady;
            if (_abilityIsReady) SetCooldown(0);
            else SetCooldown(AbilityCooldown);
        }
        public void SetSpecialRange(int specialRange)
        {
            _specialRange = specialRange;
        }
        public void SetSpecialType(RangesType rangesType)
        {
            _specialRangeType = rangesType;
        }
        public void SetAbilityDescription(string abilityDescription)
        {
            _abilityDescription = abilityDescription;
        }
        #endregion
    }
}