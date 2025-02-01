using System;
using Core.Interface.Controllers;
using Managers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Visual.Pieces
{
    public class PieceView : MonoBehaviour
    {
        [SerializeField] public Sprite defaultSprite;
        [SerializeField] public Image PieceImage;

        [SerializeField] public Sprite PieceImage256;
        
        protected IPieceController _piece;
        
        protected virtual void Awake()
        {
            if (PieceImage == null)
                PieceImage = GetComponentInChildren<Image>();
        }

        protected virtual void SetImage(Image image)
        {
            PieceImage = image;
        }

        public void Initialize(IPieceController piece)
        {
            _piece = piece;
            AddEventTriggers();
            //OnPieceInfo += GameManager.OnShowInfo;
            //OnTokenSelected += PiecesManager.PieceSelected;
        }

        public void UpdateVisuals()
        {
            if (PieceImage == null) return;
            PieceImage.sprite = defaultSprite;
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
                if (GameManager.GameState != GameStates.PieceOnBoardSelection ||
                    GameManager.Turn != _piece.PlayerID) return;
                PieceImage.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
                //OnPieceInfo?.Invoke(_piece);
            });
            
            hoverEnd.callback.AddListener((data) => 
            {
                PieceImage.transform.localScale = new Vector3(1f, 1f, 1f);
            });
            
            onClick.callback.AddListener((data) => 
            {
                if (GameManager.GameState != GameStates.PieceOnBoardSelection ||
                    GameManager.Turn != _piece.PlayerID) return;
                PieceImage.transform.localScale = new Vector3(1f, 1f, 1f);
                GameManager.OnPieceSelected?.Invoke(_piece);
            });
            
            trigger.triggers.Add(hoverStart);
            trigger.triggers.Add(hoverEnd);
            trigger.triggers.Add(onClick);
        }
    }
}