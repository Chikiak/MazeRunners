using System;
using Core.Interfaces.Entities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Views.Tokens
{
    public abstract class TokenView : MonoBehaviour, ITokenView
    {
        [SerializeField] protected Image _tokenImage;
        public Image tokenImage => _tokenImage;
        
        [SerializeField] protected Sprite _tokenImage256;
        public Sprite tokenImage256 => _tokenImage256;
        
        public Action OnTokenSelected;
        public Action<IToken> OnTokenInfo;
        private string _tokenName;
        private bool _isSelected;
        
        protected ITokenController _token;

        public void Initialize(ITokenController token)
        {
            _token = token;
            _tokenName = token.Model.TokenName;
            AddEventTriggers();
            OnTokenInfo += GameManager.OnShowInfo;
            OnTokenSelected += GameManager.PieceSelected;
        }
        
        protected virtual void Awake()
        {
            if (tokenImage == null)
                _tokenImage = GetComponentInChildren<Image>();
        }

        protected virtual void SetImage(Image image)
        {
            _tokenImage = image;
        }
        public abstract void UpdateVisuals();
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
                if (GameManager.GameState != GameStates.PieceOnBoardSelection ||
                    GameManager.Turn != _token.PlayerID) return;
                tokenImage.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
                OnTokenInfo?.Invoke(_token.Model);
            });
            
            hoverEnd.callback.AddListener((data) => 
            {
                tokenImage.transform.localScale = new Vector3(1f, 1f, 1f);
            });
            
            onClick.callback.AddListener((data) => 
            {
                if (_isSelected) return;
                if (GameManager.GameState != GameStates.PieceOnBoardSelection ||
                    GameManager.Turn != _token.PlayerID) return;
                tokenImage.transform.localScale = new Vector3(1f, 1f, 1f);
                GameManager.SelectToken(_token);
                OnTokenSelected?.Invoke();
                GameManager.OnStateChanged?.Invoke(GameStates.SelectAction);
            });
            
            trigger.triggers.Add(hoverStart);
            trigger.triggers.Add(hoverEnd);
            trigger.triggers.Add(onClick);
        }
    }
}