using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Views.Selection
{
    public class HandleSelectToken : MonoBehaviour
    {
        [SerializeField] private Image tokenImage;
        [SerializeField] private Sprite defaultSprite;
        [SerializeField] private TokensNames tokenName;
        public Action<TokensNames> OnTokenSelected;
        public Action<TokensNames> OnTokenInfo;
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
                OnTokenSelected?.Invoke(tokenName);
            });
            
            trigger.triggers.Add(hoverStart);
            trigger.triggers.Add(hoverEnd);
            trigger.triggers.Add(onClick);
        }
    }
}

