using System;
using System.Collections.Generic;
using Core.Interfaces;
using Core.Interfaces.Entities;
using Core.Models.Traps;
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
        [SerializeField] private List<TrapTypes> _trapOrder;
        [SerializeField] private List<Sprite> _trapSprites;
        private static Dictionary<TrapTypes, Sprite> _trapSpritesDict;
        
        private void Awake()
        {
            LoadDictionary();
        }
        private void LoadDictionary()
        {
            _trapSpritesDict = new Dictionary<TrapTypes, Sprite>();
            if (_trapSprites.Count != _trapOrder.Count) throw new Exception($"Trap view bad setup");
            for (int i = 0; i < _trapSprites.Count; i++)
            {
                _trapSpritesDict.Add(_trapOrder[i], _trapSprites[i]);
            }
        }

        private static Sprite GetTrapSprite(TrapTypes trapType)
        {
            Sprite trapSprite = _trapSpritesDict[trapType];
            return trapSprite;
        }
        
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
            
            var customFloorImage = customFloor.GetComponent<Image>();
            customFloorImage.color = new Color(1f, 1f, 1f, 0f);
            if (cell.Type == TrapTypes.NoTrap) return;
            customFloorImage.color = new Color(1f, 1f, 1f, 1f);
            customFloorImage.sprite = GetTrapSprite(cell.Type);
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
                //Agregar tokens
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