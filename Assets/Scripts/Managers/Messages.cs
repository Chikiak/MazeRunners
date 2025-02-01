using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Managers
{
    public class Messages : MonoBehaviour
    {
        private void Awake()
        {
            AddEventTrigger();
        }

        private void AddEventTrigger()
        {
            EventTrigger trigger = this.AddComponent<EventTrigger>();
            EventTrigger.Entry onClick = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerClick
            };
            onClick.callback.AddListener((data) => 
            {
                this.gameObject.SetActive(false);
            });
            trigger.triggers.Add(onClick);
        }
    }
}