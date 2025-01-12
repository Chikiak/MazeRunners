namespace Core.Interfaces.Entities
{
    public interface IHasAbility
    {
        
        bool AbilityIsReady { get; }
        int CurrentCooldown { get; }
        int AbilityCooldown { get; }
        RangesType SpecialRangeType { get; }
        int SpecialRange { get; }
        string AbilityDescription { get; }
        
        void SetCooldown(int newCooldown);
        void SetAbilityCooldown(int cooldown);
        void SetAbilityIsReady(bool isReady);
        void SetSpecialRange(int specialRange);
        void SetSpecialType(RangesType rangeType);
        void SetAbilityDescription(string abilityDescription);
    }
}