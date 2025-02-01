using System;
using System.Collections.Generic;
using Core.Interface.Controllers;
using Core.Interface.Models;

namespace Core.Controllers
{
    public partial class CubeController
    {
        #region Points

        private static Random _random = new Random();
        private int[] _pointsValue = { 1, 3, 5 };

        public void InitializePoints()
        {
            AddInitialPoints();
        }

        private void AddInitialPoints()
        {
            var currentPoints = _remainingPoints;
            while (currentPoints > 0)
            {
                Random rnd = new Random();
                int face = rnd.Next(6);
                int posX = rnd.Next(_size);
                int posY = rnd.Next(_size);
                int points = rnd.Next(100);
                if (points < 50)
                {
                    points = 1;
                }
                else if (points < 80)
                {
                    points = 3;
                }
                else
                {
                    points = 5;
                }

                if (points > currentPoints) points = currentPoints;

                var newCell = Model.Cells[face][posX, posY];
                newCell.SetPoints(points);
                currentPoints -= points;
            }
        }

        private void AddPoint(ICell cell, int points)
        {
            cell.SetPoints(cell.Points + points);
            _remainingPoints -= points;
        }

        public void AddPoint(int faceIndex, (int x, int y) position, int points)
        {
            ICell cell = Model.Cells[faceIndex][position.x, position.y];
            AddPoint(cell, points);
        }

        public void PointsPieceToPosition(IPieceController piece)
        {
            AddPoint(0, piece.Position, piece.PieceModel.Points);
            piece.PieceModel.SetPoints(0);
        }

        #endregion
    }
}