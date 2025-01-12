using System;
using Core.Interfaces.Entities;
using UnityEngine;
using UnityEngine.UI;
using Views.Tokens;

namespace Views.Info
{
    public class InfoPanel : MonoBehaviour
    {
        [SerializeField] private Image _image256;
        [SerializeField] private GameObject _info;

        private void Awake()
        {
            GameManager.OnShowInfo += ShowInfo;
        }

        private void ShowInfo(IToken token)
        {
            _image256.sprite = token.TokenPrefab.GetComponent<TokenView>().tokenImage256;
        }
    }
}