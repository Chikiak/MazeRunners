using System;
using Core.Interfaces.Entities;
using Core.Models.Entities.SO;
using UnityEngine;

namespace Views.Selection
{
    public class SelectionPanel : MonoBehaviour
    {
        [SerializeField] private GameObject[] _tokenSelectPrefab;
        private ITokenController _tokenSelected;
        public SOsManager sosManager;
        public Action<TokensNames> OnTokenSelected;
        public Action<TokensNames> OnTokenInfo;

        private void Start()
        {
            foreach (var token in _tokenSelectPrefab)
            {
                var newGameObj = Instantiate(token, transform);
                var handle = newGameObj.GetComponent<HandleSelectToken>();
                handle.OnTokenSelected += OnTokenSelected;
                handle.OnTokenInfo += OnTokenInfo;
            }
        }

        private void HandleTokenSelected(string tokenName)
        {
            /*if (tokenName == "Healer")
            {
                var newModel = sosManager.GetToken(tokenName);
                _tokenSelected = new HealerController(newModel, GameManager.Turn);
            }
            else if (tokenName == "Destroyer")
            {
                var newModel = sosManager.GetToken(tokenName);
                _tokenSelected = new DestroyerController(newModel, GameManager.Turn);
            }
            else throw new Exception($"Token {tokenName} is not supported");*/
            
            //OnTokenSelected?.Invoke(_tokenSelected);
        }
        private void HandleTokenInfo(string tokenName)
        {
            /*if (tokenName == "Healer")
            {
                var newModel = sosManager.GetToken(tokenName);
                _tokenSelected = new HealerController(newModel, GameManager.Turn);
            }
            else if (tokenName == "Destroyer")
            {
                var newModel = sosManager.GetToken(tokenName);
                _tokenSelected = new DestroyerController(newModel, GameManager.Turn);
            }
            else throw new Exception($"Token {tokenName} is not supported");*/

            //OnTokenInfo?.Invoke(_tokenSelected);
        }
    }
}