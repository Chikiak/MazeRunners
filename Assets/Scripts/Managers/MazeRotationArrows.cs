using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Managers
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


        private void CreateArrow(Transform parent, int index, bool isRow, bool clockwise)
        {
            GameObject arrowObj = Instantiate(arrowPrefab, parent);
        
            RectTransform rectTransform = arrowObj.GetComponentInChildren<RectTransform>();
            rectTransform.sizeDelta = new Vector2(arrowSize, arrowSize);
            
            float rotation = 0f;
            if (isRow) 
            {
                rotation = clockwise ? 180f : 0f;
            }
            else
            {
                rotation = clockwise ? 270f : 90f;
            }
            rectTransform.rotation = Quaternion.Euler(0, 0, rotation);
            AddEventTrigger(arrowObj, index, isRow, clockwise);
            Debug.Log("Created arrow");
            
        }
        
        private void AddEventTrigger(GameObject arrowObj, int index, bool isRow, bool clockwise)
        {
            Debug.Log($"Adding event {arrowObj.name}");
            EventTrigger trigger = arrowObj.AddComponent<EventTrigger>();
            EventTrigger.Entry onClick = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerClick
            };
            onClick.callback.AddListener((data) => 
            {
                GameManager.OnRotate?.Invoke(isRow, clockwise, index);
                Debug.Log($"OnRotate called {arrowObj.name}");
            });
            
            trigger.triggers.Add(onClick);
        }
        
        public void InitializeArrows(int size)
        {
            Debug.Log("InitializeArrows");
            ClearArrows();
            float pos = ((arrowSize * size / 2) + arrowSize / 2);
            rowArrowsLeft.SetLocalPositionAndRotation(new Vector3(-1 * pos, pos - arrowSize, 0f), Quaternion.identity);
            rowArrowsRight.SetLocalPositionAndRotation(new Vector3(pos, pos - arrowSize, 0f), Quaternion.identity);
            columnArrowsTop.SetLocalPositionAndRotation(new Vector3(-1 * (pos - arrowSize), pos, 0f), Quaternion.identity);
            columnArrowsBottom.SetLocalPositionAndRotation(new Vector3(-1 * (pos - arrowSize), -1 * pos, 0f), Quaternion.identity);
            
            for (int i = 0; i < size; i++)
            {
                CreateArrow(rowArrowsLeft, i, true, true);
                CreateArrow(rowArrowsRight, i, true, false);  
                CreateArrow(columnArrowsTop, i, false, false);  
                CreateArrow(columnArrowsBottom, i, false, true); 
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