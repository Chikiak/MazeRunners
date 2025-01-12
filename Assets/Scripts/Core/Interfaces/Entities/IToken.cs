using UnityEngine;

namespace Core.Interfaces.Entities
{
    public interface IToken : IDamageable, IMovable, IHasAbility
    {
        string TokenName { get; }
        GameObject TokenPrefab { get; }
        int Dmg { get; }
        TokensNames Name { get; }
        
        TokensStates State { get; }
        int TokenIndex { get; }
    }
}