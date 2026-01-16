using System;
using System.Collections.Generic;
using Core.Interface.Controllers;
using Managers;

namespace Core.Abilities
{
    /// <summary>
    /// Factory class for creating piece abilities using Factory Pattern.
    /// </summary>
    public static class AbilityFactory
    {
        private static Dictionary<PieceType, Func<IMazeGenerator, IAbility>> _abilityCreators;
        
        static AbilityFactory()
        {
            _abilityCreators = new Dictionary<PieceType, Func<IMazeGenerator, IAbility>>
            {
                { PieceType.Healer, _ => new HealerAbility() },
                { PieceType.Destroyer, mazeGen => new DestroyerAbility(mazeGen) },
                { PieceType.Lancer, _ => new LancerAbility() },
                { PieceType.Gladiator, _ => new GladiatorAbility() },
                { PieceType.Thief, _ => new ThiefAbility() },
                { PieceType.Explorer, _ => new ExplorerAbility() },
                { PieceType.Archer, _ => new ArcherAbility() },
                { PieceType.Tank, _ => new TankAbility() }
            };
        }
        
        /// <summary>
        /// Creates an ability for the specified piece type.
        /// </summary>
        /// <param name="pieceType">The type of piece.</param>
        /// <param name="mazeGenerator">The maze generator (required for some abilities).</param>
        /// <returns>The ability instance.</returns>
        public static IAbility CreateAbility(PieceType pieceType, IMazeGenerator mazeGenerator = null)
        {
            if (_abilityCreators.TryGetValue(pieceType, out var creator))
            {
                return creator(mazeGenerator);
            }
            
            throw new ArgumentOutOfRangeException(nameof(pieceType), $"No ability defined for piece type: {pieceType}");
        }
    }
}
