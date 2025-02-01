using System;
using Core.Controllers;
using Core.Interface.Controllers;
using Core.Interface.Models;
using Managers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Visual.Selection
{
    public class HandleSelectPiece : MonoBehaviour
    {
        [SerializeField] private Image tokenImage;
        [SerializeField] private Sprite defaultSprite;
        [SerializeField] private PieceType tokenName;
        public Action<PieceType> OnTokenInfo;
        private bool _isSelected;
        
        public void UpdateVisuals()
        {
            if (tokenImage == null) return;
            tokenImage.sprite = defaultSprite;
        }
        
        private void Awake()
        {
            if (tokenImage == null)
                tokenImage = GetComponentInChildren<Image>();
            UpdateVisuals();
            AddEventTriggers();
        }
        
        private void AddEventTriggers()
        {
            EventTrigger trigger = this.AddComponent<EventTrigger>();
            EventTrigger.Entry hoverStart = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerEnter
            };
            EventTrigger.Entry hoverEnd = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerExit
            };
            EventTrigger.Entry onClick = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerClick
            };
            
            hoverStart.callback.AddListener((data) => 
            {
                if (_isSelected) return;
                if (GameManager.GameState != GameStates.SelectInitialPiece) return;
                tokenImage.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
                OnTokenInfo?.Invoke(tokenName);
            });
            
            hoverEnd.callback.AddListener((data) => 
            {
                tokenImage.transform.localScale = new Vector3(1f, 1f, 1f);
            });
            
            onClick.callback.AddListener((data) => 
            {
                if (_isSelected) return;
                if (GameManager.GameState != GameStates.SelectInitialPiece) return;
                tokenImage.transform.localScale = new Vector3(1f, 1f, 1f);
                tokenImage.color = new Color(0f, 0f, 0f, 0.5f);
                _isSelected = true;
                IPieceController piece = new PieceController();
                IPieceModel pieceModel = PiecesInitialData.GetInitialPiece(tokenName);
                piece.Initialize(pieceModel, GameManager.Turn);
                GameManager.OnPieceSelected?.Invoke(piece);
            });
            
            trigger.triggers.Add(hoverStart);
            trigger.triggers.Add(hoverEnd);
            trigger.triggers.Add(onClick);
        }
    }
}