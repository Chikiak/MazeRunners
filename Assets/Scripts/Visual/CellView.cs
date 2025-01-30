using Core.Interface.Models;
using Core.Interface.Visual;
using UnityEngine;
using UnityEngine.UI;

namespace Visual
{
    public class CellView : ACellView
    {
        private void Awake()
        {
            LoadDictionary();
        }
        public override void UpdateCell(ICell cell)
        {
            if (cell.Position.Item1 % 2 == cell.Position.Item2 % 2)
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
        }
    }
}