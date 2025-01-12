using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Views.Selection
{
    public class HandleSelectCell : MonoBehaviour
    {
        private Image layoutImage;
        private (int,int) _position;
        public (int,int) Position => _position;
        public Action<(int, int)> OnCellSelected;

        public void SetPosition((int, int) newPosition)
        {
            _position = newPosition;
        }
        
        private void Awake()
        {
            AddEventTriggers();
            layoutImage = GetComponent<Image>();
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
                if (GameManager.GameState != GameStates.CellSelection &&
                    GameManager.GameState != GameStates.PutingInitialPiece) return;
                layoutImage.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            });
            
            hoverEnd.callback.AddListener((data) => 
            {
                layoutImage.transform.localScale = new Vector3(1f, 1f, 1f);
            });
            
            onClick.callback.AddListener((data) => 
            {
                if (GameManager.GameState != GameStates.CellSelection &&
                    GameManager.GameState != GameStates.PutingInitialPiece) return;
                OnCellSelected?.Invoke(_position);
            });
            
            trigger.triggers.Add(hoverStart);
            trigger.triggers.Add(hoverEnd);
            trigger.triggers.Add(onClick);
        }
    }
}