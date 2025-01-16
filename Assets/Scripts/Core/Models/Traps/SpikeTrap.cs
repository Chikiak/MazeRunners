using Core.Interfaces;

namespace Core.Models.Traps
{
    public class SpikeTrap : Trap
    {
        public SpikeTrap((int, int) position) : base(position)
        {
            SetType(TrapTypes.Spikes);
            SetAbilityCooldown(3);
            SetAbilityIsReady(true);
            SetSpecialType(RangesType.Square);
            SetSpecialRange(1);
            SetAbilityDescription("The pieces take damage when walk trough this trap");
        }

        public SpikeTrap(ICell cell) : base(cell) {}

    }
}