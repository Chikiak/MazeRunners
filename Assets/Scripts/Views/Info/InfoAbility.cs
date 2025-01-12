using Core.Interfaces.Entities;
using TMPro;
using UnityEngine;

namespace Views.Info
{
    public class InfoAbility : MonoBehaviour
    {
        [SerializeField] private TMP_Text _abilityText;
        private void Awake()
        {
            GameManager.OnShowInfo += ShowInfo;
        }

        private void ShowInfo(IToken token)
        {
            _abilityText.text = "Description:\n" + token.AbilityDescription;
        }
    }
}