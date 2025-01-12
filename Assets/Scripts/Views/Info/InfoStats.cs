using Core.Interfaces.Entities;
using TMPro;
using UnityEngine;

namespace Views.Info
{
    public class InfoStats : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _healthText;
        [SerializeField] private TMP_Text _atkText;
        [SerializeField] private TMP_Text _speedText;
        [SerializeField] private TMP_Text _cooldownText;

        
        private void Awake()
        {
            GameManager.OnShowInfo += ShowInfo;
        }
        private void ShowInfo(IToken token)
        {
            _nameText.text = $"{token.TokenName}";
            _healthText.text = $"HP: {token.Health} / {token.MaxHealth}";
            _atkText.text = $"Atk: {token.Dmg}";
            _speedText.text = $"Speed: {token.Speed}";
            _cooldownText.text = $"Cooldown: {token.CurrentCooldown} / {token.AbilityCooldown}";
            _cooldownText.autoSizeTextContainer = true;
        }
    }
}