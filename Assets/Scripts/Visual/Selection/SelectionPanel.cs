using System;
using Core.Interface.Controllers;
using Managers;
using UnityEngine;

namespace Visual.Selection
{
    public class SelectionPanel : MonoBehaviour
    {
        [SerializeField] private GameObject[] _tokenSelectPrefab;
        private IPieceController _tokenSelected;
        public Action<PieceType> OnTokenInfo;

        private void Start()
        {
            foreach (var token in _tokenSelectPrefab)
            {
                var newGameObj = Instantiate(token, transform);
                var handle = newGameObj.GetComponent<HandleSelectPiece>();
                handle.OnTokenInfo += OnTokenInfo;
            }
        }
    }
}