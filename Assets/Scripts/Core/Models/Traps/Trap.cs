using System;
using Core.Interfaces;
using Core.Interfaces.Entities;

namespace Core.Models.Traps
{
    public abstract class Trap : Cell, IHasAbility
    {
        protected Trap((int, int) position) : base(position) { }
        protected Trap(ICell cell) : base(cell)
        {
            if (!(cell is Trap)) throw new Exception("Trap: The Cell is not Trap");
            var ncell = cell as Trap;
            SetType(cell.Type);
            SetAbilityCooldown(ncell.AbilityCooldown);
            SetAbilityIsReady(ncell.AbilityIsReady);
            SetCooldown(ncell.CurrentCooldown);
            SetSpecialType(ncell.SpecialRangeType);
            SetSpecialRange(ncell.SpecialRange);
            SetAbilityDescription(ncell.AbilityDescription);
            
        }
        #region Properties
        protected bool _abilityIsReady;
        public bool AbilityIsReady => _abilityIsReady;
        
        protected int _currentCooldown;
        public int CurrentCooldown => _currentCooldown;
        
        protected int _abilityCooldown;
        public int AbilityCooldown => _abilityCooldown;

        protected RangesType _specialRangeType;
        public RangesType SpecialRangeType => _specialRangeType;
        
        protected int _specialRange;
        public int SpecialRange => _specialRange;
        
        protected string _abilityDescription;
        public string AbilityDescription => _abilityDescription;
        #endregion
        
        #region Setters
        public void SetCooldown(int newCooldown)
        {
            _currentCooldown = newCooldown;
            if (_currentCooldown > AbilityCooldown) _currentCooldown = AbilityCooldown;
            if (_currentCooldown <= 0)
            {
                _currentCooldown = 0;
                SetAbilityIsReady(true);
            }
        }
        public void SetAbilityCooldown(int cooldown)
        {
            _abilityCooldown = cooldown;
        }

        public void SetAbilityIsReady(bool isReady)
        {
            _abilityIsReady = isReady;
            if (!isReady) SetCooldown(AbilityCooldown);
        }

        public void SetSpecialRange(int newSpecialRange)
        {
            _specialRange = newSpecialRange;
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

        public override void ApplyEffects()
        {
            if (!AbilityIsReady) return;
            GameManager.ActivateTrap?.Invoke(Position, Type);
        }
    }
}