using System;
using System.Collections.Generic;
using Core.Interface.Models;
using Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core.Interface.Visual
{
    public abstract class ACellView : MonoBehaviour
    {
        [SerializeField] protected GameObject[] walls = new GameObject[4];
        [SerializeField] protected GameObject floor;
        [SerializeField] protected GameObject customFloor;
        [SerializeField] protected GameObject pointsView;
        [SerializeField] protected GameObject selectableLayout;
        public Action<(int,int)> OnCellClicked;
        [SerializeField] private List<TrapType> trapOrder;
        [SerializeField] private List<Sprite> trapSprites;
        protected static Dictionary<TrapType, Sprite> TrapSpritesDict;
        
        public abstract void UpdateCell(ICell cell);
        
        
        protected void LoadDictionary()
        {
            TrapSpritesDict = new Dictionary<TrapType, Sprite>();
            if (trapSprites.Count != trapOrder.Count) throw new Exception($"Trap view bad setup");
            for (int i = 0; i < trapSprites.Count; i++)
            {
                TrapSpritesDict.Add(trapOrder[i], trapSprites[i]);
            }
        }
    }
}