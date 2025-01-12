using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Views
{
    public class MazeRotationArrows : MonoBehaviour
    {
        [Header("Arrow Prefabs")]
        [SerializeField] private GameObject arrowPrefab;
    
        [Header("Container References")]
        [SerializeField] private Transform rowArrowsLeft;
        [SerializeField] private Transform rowArrowsRight;
        [SerializeField] private Transform columnArrowsTop;
        [SerializeField] private Transform columnArrowsBottom;
    
        [Header("Arrow Settings")]
        [SerializeField] private float arrowSize = 64f;
        [SerializeField] private float arrowOffset = 0f;

        public static event Action<int, bool> OnRowRotate; // (rowIndex, clockwise)
        public static event Action<int, bool> OnColumnRotate; // (columnIndex, clockwise)

        private void CreateArrow(Transform parent, int index, bool isRow, bool clockwise)
        {
            GameObject arrowObj = Instantiate(arrowPrefab, parent);
        
            RectTransform rectTransform = arrowObj.GetComponentInChildren<RectTransform>();
            rectTransform.sizeDelta = new Vector2(arrowSize, arrowSize);
            
            float rotation = 0f;
            if (isRow) 
            {
                rotation = clockwise ? 0f : 180f;
            }
            else
            {
                rotation = clockwise ? 90f : 270f;
            }
            rectTransform.rotation = Quaternion.Euler(0, 0, rotation);
            AddEventTrigger(arrowObj, index, isRow, clockwise);
            
        }
        
        private void AddEventTrigger(GameObject arrowObj, int index, bool isRow, bool clockwise)
        {
            EventTrigger trigger = arrowObj.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            
            entry.callback.AddListener((data) =>
            {
                Debug.Log($"{GameManager.GameState}, {GameManager.MazeChanged}");
                if (GameManager.GameState != GameStates.PieceOnBoardSelection || GameManager.MazeChanged) return;
                if (isRow)
                {
                    OnRowRotate?.Invoke(index, clockwise);
                }
                else
                {
                    OnColumnRotate?.Invoke(index, clockwise);
                }
            });
            
            trigger.triggers.Add(entry);
        }
        
        public void InitializeArrows(int size)
        {
            ClearArrows();
            float pos = ((arrowSize * size / 2) + arrowSize / 2);
            rowArrowsLeft.SetLocalPositionAndRotation(new Vector3(-1 * pos, pos - arrowSize, 0f), Quaternion.identity);
            rowArrowsRight.SetLocalPositionAndRotation(new Vector3(pos, pos - arrowSize, 0f), Quaternion.identity);
            columnArrowsTop.SetLocalPositionAndRotation(new Vector3(-1 * (pos - arrowSize), pos, 0f), Quaternion.identity);
            columnArrowsBottom.SetLocalPositionAndRotation(new Vector3(-1 * (pos - arrowSize), -1 * pos, 0f), Quaternion.identity);
            
            for (int i = 0; i < size; i++)
            {
                CreateArrow(rowArrowsLeft, i, true, false);
                CreateArrow(rowArrowsRight, i, true, true);  
                CreateArrow(columnArrowsTop, i, false, true);  
                CreateArrow(columnArrowsBottom, i, false, false); 
            }
        }

        private void ClearArrows()
        {
            ClearContainer(rowArrowsLeft);
            ClearContainer(rowArrowsRight);
            ClearContainer(columnArrowsTop);
            ClearContainer(columnArrowsBottom);
        }

        private void ClearContainer(Transform container)
        {
            foreach (Transform child in container)
            {
                Destroy(child.gameObject);
            }
        }
    }
}