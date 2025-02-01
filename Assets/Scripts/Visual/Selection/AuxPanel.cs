using System;
using Core.Interface.Controllers;
using Managers;
using UnityEngine;

namespace Visual.Selection
{
    public class AuxPanel : MonoBehaviour
    {

        [SerializeField] private GameObject _selectionPanel;
        [SerializeField] private GameObject _actionsPanel;
        private GameObject _selectPanelInstance;
        private GameObject _actionsPanelInstance;
        private SelectionPanel _selection;
        public Action<IPieceController> OnTokenSelected;
        public Action<PieceType> OnTokenInfo;

        private void Awake()
        {
            GameManager.OnStateChanged += HandleStateChanged;
        }
        
        private void OnEnable()
        {
            _selectPanelInstance = Instantiate(_selectionPanel, transform);
            _selectionPanel.SetActive(true);
            _actionsPanelInstance = Instantiate(_actionsPanel, transform);
            _actionsPanelInstance.SetActive(false);
            
            _selection = _selectPanelInstance.GetComponent<SelectionPanel>();
            _selection.OnTokenInfo += OnTokenInfo;
        }
        private void OnDisable()
        {
            _selection.OnTokenInfo -= OnTokenInfo;
            GameManager.OnStateChanged -= HandleStateChanged;
        }

        private void HandleStateChanged(GameStates newState)
        {
            if (newState == GameStates.SelectInitialPiece || newState == GameStates.PutingInitialPiece)
            {
                _selectPanelInstance.SetActive(true);
                _actionsPanelInstance.SetActive(false);
            }
            else if (newState == GameStates.SelectAction)
            {
                _selectPanelInstance.SetActive(false);
                _actionsPanelInstance.SetActive(true);
            }
            else
            {
                _selectPanelInstance.SetActive(false);
                _actionsPanelInstance.SetActive(false);
            }
        }
    }
}