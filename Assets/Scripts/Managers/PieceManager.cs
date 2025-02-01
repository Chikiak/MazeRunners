using System.Collections.Generic;
using Core.Controllers;
using Core.Interface.Controllers;
using Core.Interface.Models;
using UnityEngine;
using Random = System.Random;

namespace Managers
{
    public static class PieceManager
    {
        public static List<IPieceController>[,] PiecesMatrix { get; private set; }
        public static List<IPieceController> DefeatedPieces { get; private set; }
        public static IPieceController SelectedPiece { get; private set; }

        public static void PiecesNewTurn()
        {
            for (int x = 0; x < PiecesMatrix.GetLength(0); x++)
            {
                for (int y = 0; y < PiecesMatrix.GetLength(1); y++)
                {
                    if (PiecesMatrix[x, y].Count > 0)
                    {
                        for (int i = 0; i < PiecesMatrix[x, y].Count; i++)
                        {
                            var piece = PiecesMatrix[x, y][i];
                            piece.ReduceCooldowns();
                            if (!piece.IsAlive()) DefeatedPieces.Add(PiecesMatrix[x, y][i]);
                        }
                    }
                    //Attack
                    if (PiecesMatrix[x, y].Count != 2) continue;
                    PiecesMatrix[x, y][0].TakeDamage(PiecesMatrix[x, y][1].GetInfo().Damage);
                    if(PiecesMatrix[x, y][0].IsAlive())
                    {
                        PiecesMatrix[x, y][1].TakeDamage(PiecesMatrix[x, y][0].GetInfo().Damage);
                        if(!PiecesMatrix[x, y][1].IsAlive())
                        {
                            DefeatedPieces.Add(PiecesMatrix[x, y][1]);
                        }
                    }
                    else
                    {
                        DefeatedPieces.Add(PiecesMatrix[x, y][0]);
                    }
                }
            }
        }
        public static void HandleDefeatedPieces()
        {
            if (SelectedPiece == null) return;
            foreach (IPieceController defeatedPiece in DefeatedPieces)
            {
                DefeatedPiece(defeatedPiece);
            }
            DefeatedPieces.Clear();
        }
        public static void Initialize(int mazeSize)
        {
            GameManager.OnPieceSelected += SetSelectedPiece;
            PiecesMatrix = new List<IPieceController>[mazeSize, mazeSize];
            DefeatedPieces = new List<IPieceController>();
            for (int x = 0; x < mazeSize; x++)
                for (int y = 0; y < mazeSize; y++)
                    PiecesMatrix[x, y] = new List<IPieceController>();
        }
        private static void SetSelectedPiece(IPieceController piece)
        {
            SelectedPiece = piece;
        }
        public static List<IPieceController> GetPiecesInCell((int x, int y) position)
        {
            return PiecesMatrix[position.x, position.y];
        }
        public static bool AddPiece(IPieceController pieceController, (int x, int y) position)
        {
            if (!(ValidatePosition(position) && ValidateMove(position))) return false;
            PiecesMatrix[position.x, position.y].Add(pieceController);
            pieceController.SetPosition((position.x, position.y));
            return true;
        }
        public static bool MovePiece(IPieceController pieceController, Direction direction)
        {
            (int x, int y) newPosition = pieceController.Position;
            switch (direction)
            {
                case Direction.Up:
                    newPosition.y--;
                    break;
                case Direction.Down:
                    newPosition.y++;
                    break;
                case Direction.Left:
                    newPosition.x--;
                    break;
                case Direction.Right:
                    newPosition.x++;
                    break;
            }

            RemovePiece(pieceController);
            AddPiece(pieceController, newPosition);
            return true;
        }
        private static void DefeatedPiece(IPieceController piece)
        {
            PiecesMatrix[piece.Position.x, piece.Position.y].Remove(piece);
            (int x, int y) newPosition = RandomInitialPosition();
            piece.Revive();
            AddPiece(piece, newPosition);
        }
        private static void RemovePiece(IPieceController pieceController)
        {
            PiecesMatrix[pieceController.Position.x, pieceController.Position.y].Remove(pieceController);
        }
        private static bool ValidatePosition((int x, int y) position)
        {
            if(position.x < 0 || position.y < 0) return false;
            if(position.x >= PiecesMatrix.GetLength(0) || position.y >= PiecesMatrix.GetLength(1)) return false;
            return true;
        }
        private static bool ValidateMove((int x, int y) position)
        {
            if(PiecesMatrix[position.x, position.y].Count > 1) return false;
            return true;
        }
        private static (int x, int y) RandomInitialPosition()
        {
            Random random = new Random();
            List<(int x, int y)> initialCells = new List<(int, int)> ();
            int center = PiecesMatrix.GetLength(0)/2;
            for(int x = -1; x < 2; x++)
            {
                for(int y = -1; y < 2; y++)
                {
                    if (x == 0 && y == 0) continue;
                    if (ValidateMove((center + x, center + y))) initialCells.Add((center + x, center + y));
                }
            }
            return initialCells[random.Next(0, initialCells.Count)];
        }
    }
}