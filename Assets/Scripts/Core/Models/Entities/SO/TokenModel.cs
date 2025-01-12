using Core.Interfaces.Entities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core.Models.Entities.SO
{
    [CreateAssetMenu(fileName = "NewTokenData", menuName = "Game/Token Data")]
    public class TokenModel : ScriptableObject, IToken
    {
        #region Properties
        [SerializeField] private string _tokenName;
        public string TokenName => _tokenName;
        
        [SerializeField] private TokensNames _name;
        public TokensNames Name => _name;
        
        [SerializeField] private GameObject _tokenPrefab;
        public GameObject TokenPrefab => _tokenPrefab;
        
        private TokensStates _state = TokensStates.Idle;
        public TokensStates State => _state;

        [SerializeField] private int _health;
        public int Health => _health;

        [SerializeField] private int _maxHealth;
        public int MaxHealth => _maxHealth;

        private (int, int) _position;
        public (int, int) Position => _position;

        [SerializeField] private int _speed;
        public int Speed => _speed;
        
        [SerializeField] private int _remainingMovs;
        public int RemainingMovs => _remainingMovs;

        private bool _abilityIsReady;
        public bool AbilityIsReady => _abilityIsReady;

        [SerializeField] private int _currentCooldown;
        public int CurrentCooldown => _currentCooldown;

        [SerializeField] private int _abilityCooldown;
        public int AbilityCooldown => _abilityCooldown;

        private int _tokenIndex;
        public int TokenIndex => _tokenIndex;

        [SerializeField] private RangesType _specialRangeType;
        public RangesType SpecialRangeType => _specialRangeType;
        [SerializeField] private int _specialRange;
        public int SpecialRange => _specialRange;

        [SerializeField] private int _dmg;
        public int Dmg => _dmg;
        
        [SerializeField] private string _abilityDescription;
        public string AbilityDescription => _abilityDescription;

        public TokenModel(IToken getToken)
        {
            SetName(getToken.TokenName);
            SetPrefab(getToken.TokenPrefab);
            SetHealth(getToken.Health);
            SetMaxHealth(getToken.MaxHealth);
            SetPosition(getToken.Position);
            SetSpeed(getToken.Speed);
            SetCooldown(getToken.CurrentCooldown);
            SetAbilityCooldown(getToken.AbilityCooldown);
            SetAbilityIsReady(getToken.AbilityIsReady);
            SetState(getToken.State);
            SetSpecialType(getToken.SpecialRangeType);
            SetSpecialRange(getToken.SpecialRange);
            SetAbilityDescription(getToken.AbilityDescription);
        }

        
        #endregion

        #region Setters

        public void SetName(string name)
        {
            _tokenName = name;
        }

        public void SetPrefab(GameObject prefab)
        {
            _tokenPrefab = prefab;
        }
        public void SetState(TokensStates state)
        {
            _state = state;
        }
        public void SetHealth(int health)
        {
            _health = health;
            if (_health > MaxHealth) _health = MaxHealth;
        }

        public void SetMaxHealth(int maxHealth)
        {
            _maxHealth = maxHealth;
        }

        public void SetPosition((int, int) newPosition)
        {
            _position = newPosition;
        }

        public void SetSpeed(int newSpeed)
        {
            _speed = newSpeed;
        }

        public void SetRemainingMovs(int newRemainingMovs)
        {
            _remainingMovs = newRemainingMovs;
            if (_remainingMovs > _speed) _remainingMovs = _speed;
            if (_remainingMovs < 0) _remainingMovs = 0;
        }

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

        public void SetSpecialType(RangesType rangesType)
        {
            _specialRangeType = rangesType;
        }
        public void SetSpecialRange(int newSpecialRange)
        {
            _specialRange = newSpecialRange;
        }

        public void SetAbilityDescription(string abilityDescription)
        {
            _abilityDescription = abilityDescription;
        }

        #endregion
    }
}