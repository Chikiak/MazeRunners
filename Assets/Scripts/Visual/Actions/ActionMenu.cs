using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Visual.Actions
{
    public class ActionsMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _moveButton;
        [SerializeField] private GameObject _abilityButton;
        [SerializeField] private GameObject _endTurnButton;

        private void Start()
        {
            AddEventTriggers(_moveButton);
            AddEventTriggers(_abilityButton);
            AddEventTriggers(_endTurnButton);
        }
        private void AddEventTriggers(GameObject obj)
        {
            EventTrigger trigger = obj.AddComponent<EventTrigger>();
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
                obj.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            });
            
            hoverEnd.callback.AddListener((data) => 
            {
                obj.transform.localScale = new Vector3(1f, 1f, 1f);
            });
            
            onClick.callback.AddListener((data) => 
            {
                obj.transform.localScale = new Vector3(1f, 1f, 1f);
                ButtonFunctionality(obj);
            });
            
            trigger.triggers.Add(hoverStart);
            trigger.triggers.Add(hoverEnd);
            trigger.triggers.Add(onClick);
        }

        private void ButtonFunctionality(GameObject obj)
        {
            MoveButton(obj);
        }

        private void MoveButton(GameObject obj)
        {
            if (obj == _moveButton)
            {
                var piece = PieceManager.SelectedPiece;
                if (!piece.IsAlive())
                {
                    Debug.Log("Selected token is dead");
                    return;
                }
                if (piece.PieceModel.RemainingMovs <= 0)
                {
                    Debug.Log("No more moves left");
                    return;
                }
                GameManager.OnChangeActualAction?.Invoke(ActionType.Move);
                GameManager.OnStateChanged?.Invoke(GameStates.CellSelection);
                GameManager.SelectingCell?.Invoke();
            }
            else
            {
                AbilityButton(obj);
            }
        }

        private void AbilityButton(GameObject obj)
        {
            if (obj == _abilityButton)
            {
                var piece = PieceManager.SelectedPiece;
                if (!piece.IsAlive())
                {
                    Debug.Log("Selected token is dead");
                    return;
                }

                if (piece.PieceModel.RemainingMovs < piece.PieceModel.Speed)
                {
                    Debug.Log("Cant use Ability, you already move");
                    return;
                }
                GameManager.OnChangeActualAction?.Invoke(ActionType.UseAbility);
                piece.UseAbility();
            }
            else
            {
                EndButton(obj);
            }
        }

        private void EndButton(GameObject obj)
        {
            if (obj == _endTurnButton)
            {
                GameManager.OnNewTurn?.Invoke();
            }
        }
    }
}