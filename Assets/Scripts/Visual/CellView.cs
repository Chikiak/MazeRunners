using System;
using System.Collections.Generic;
using Core.Controllers;
using Core.Interface.Controllers;
using Core.Interface.Models;
using Core.Interface.Visual;
using Managers;
using UnityEngine;
using UnityEngine.UI;
using Visual.Pieces;
using Visual.Selection;

namespace Visual
{
    public class CellView : ACellView
    {
        [SerializeField] private List<PieceType> PiecesOrder;   
        [SerializeField] private List<GameObject> PiecesPrefabs;
        private Dictionary<PieceType, GameObject> PrefabDictionary;
        private List<GameObject> _piecesView = new List<GameObject>();
        
        private void Awake()
        {
            LoadDictionary();
            LoadPieceDictionary();
        }
        public override void UpdateCell(ICell cell)
        {
            var handle = selectableLayout.GetComponent<HandleSelectCell>();
            if (cell.Position.x % 2 == cell.Position.y % 2)
            {
                floor.GetComponent<Image>().color = new Color32(50, 125, 50, 175);
                
            }

            bool[] wallStates = new bool[] { true,true,true,true };
            foreach (var wall in cell.Walls)
            {
                wallStates[(int)wall.Key] = wall.Value;
                
            }
            for (int i = 0; i < 4; i++)
            {
                walls[i].SetActive(wallStates[i]);
            }
            if (cell.IsSelectable)
            {
                selectableLayout.SetActive(true);
                handle.SetPosition(cell.Position);
            }
            else
            {
                selectableLayout.SetActive(false);
            }
            switch (cell.Trap.TrapType)
            {
                case TrapType.Nothing:
                    customFloor.GetComponent<Image>().color = new Color32(50, 125, 50, 0);
                    break;
                case TrapType.Spikes:
                    customFloor.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                    customFloor.GetComponent<Image>().sprite = TrapSpritesDict[cell.Trap.TrapType];
                    break;
                default:
                    throw new Exception($"CellView: Invalid trap type: {cell.Trap.TrapType}");
            }
            UpdatePiecesInCell(PieceManager.PiecesMatrix[cell.Position.x, cell.Position.y]);
            UpdatePointsView(cell.Points);
        }

        private void UpdatePiecesInCell(List<IPieceController> pieceControllers)
        {
            ClearTokensView();
            if (pieceControllers == null || pieceControllers.Count == 0) return;
            foreach (var pieceController in pieceControllers)
            {
                //Agregar tokens
                var viewPrefab = PrefabDictionary[pieceController.PieceModel.PieceType];
                var newPiece = Instantiate(viewPrefab, piecesParent.transform);
                _piecesView.Add(newPiece);
                var view = newPiece.GetComponent<PieceView>();
                view.Initialize(pieceController);
                view.UpdateVisuals();
                
                //Cambiarles el color en dependencia del jugador
                var image = view.PieceImage;
                image.color = pieceController.PlayerID switch
                {
                    PlayerID.Player1 => Color.white,
                    PlayerID.Player2 => new Color(0.2f, 0.2f, 0.2f, 1f),
                    _ => Color.red
                };
            }
        }
        private void UpdatePointsView(int points)
        {
            var pointsImage = pointsView.GetComponent<Image>();
            if (pointsImage == null) return;
            if (points <= 0)
            {
                pointsImage.color = new Color(1f, 1f, 1f, 0f);
            }
            else if (points <= 1)
            {
                pointsImage.color = new Color(0.7f, 0.5f, 0f, 1f);
            } 
            else if (points <= 3)
            {
                pointsImage.color = new Color(0.7f, 0.7f, 0.7f, 1f);
            }
            else if (points <= 5)
            {
                pointsImage.color = new Color(1f, 1f, 0f, 1f);
            }
            else if (points <= 10)
            {
                pointsImage.color = new Color(0.7f, 1f, 1f, 1f);
            }
        }
        
        private void ClearTokensView()
        {
            if (_piecesView == null) return;
            foreach (var view in _piecesView)
            {
                Destroy(view);
            }
            _piecesView.Clear();
        }

        private void LoadPieceDictionary()
        {
            PrefabDictionary = new Dictionary<PieceType, GameObject>();
            for (int i = 0; i < PiecesOrder.Count; i++)
            {
                PrefabDictionary[PiecesOrder[i]] = PiecesPrefabs[i];
            }
        }
    }
}