using System;
using System.Collections.Generic;
using Core.Interfaces;
using Core.Interfaces.Entities;
using Unity.VisualScripting;
using Unity.XR.OpenVR;
using UnityEngine;
using UnityEngine.UI;
using Views.Selection;

namespace Views
{
    public class CellView : MonoBehaviour
    {
        [SerializeField] private GameObject[] walls = new GameObject[4];
        [SerializeField] private GameObject floor;
        [SerializeField] private GameObject customFloor;
        [SerializeField] private GameObject tokensGrid;
        [SerializeField] private GameObject SelectableLayout;
        private List<GameObject> _tokensView = new List<GameObject>();
        public Action<(int,int)> OnCellClicked;
        
        public void UpdateCell(ICell cell)
        {
            var handle = SelectableLayout.GetComponent<HandleSelectCell>();
            if (cell.Position.Item1 % 2 == cell.Position.Item2 % 2)
            {
                floor.GetComponent<Image>().color = new Color(0.2f, 0.5f, 0.2f, 0.7f);
            }
            bool[] wallStates = cell.Walls;
            for (int i = 0; i < 4; i++)
            {
                walls[i].SetActive(wallStates[i]);
            }

            if (cell.Selectable)
            {
                SelectableLayout.SetActive(true);
                handle.OnCellSelected += HandleSelectedCell;
                handle.SetPosition(cell.Position);
            }
            else
            {
                SelectableLayout.SetActive(false);
                handle.OnCellSelected -= HandleSelectedCell;
            }
            UpdateTokensInCell(cell.Tokens);
        }
        private void UpdateTokensInCell(List<ITokenController> tokens)
        {
            ClearTokensView();
            if (tokens == null || tokens.Count == 0) return;
            if (tokens.Count > 2)
            {
                Debug.LogError("Invalid number of tokens in a cell");
                return;
            }
            foreach (var tokenController in tokens)
            {
                //Agregar Tokens
                var newToken = Instantiate(tokenController.Model.TokenPrefab, tokensGrid.transform);
                _tokensView.Add(newToken);
                tokenController.SetView(newToken.GetComponent<ITokenView>());
                tokenController.View.Initialize(tokenController);
                tokenController.View.UpdateVisuals();
                
                //Cambiarles el color en dependencia del jugador
                var image = tokenController.View.tokenImage;
                image.color = tokenController.PlayerID switch
                {
                    Players.Player1 => Color.white,
                    Players.Player2 => new Color(0.2f, 0.2f, 0.2f, 1f),
                    _ => Color.red
                };
            }
        }
        private void ClearTokensView()
        {
            if (_tokensView == null) return;
            foreach (var view in _tokensView)
            {
                Destroy(view);
            }
            _tokensView.Clear();
        }

        private void HandleSelectedCell((int, int) position)
        {
            OnCellClicked?.Invoke(position);
        }
        
    }
}