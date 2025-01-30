using System;
using System.Collections.Generic;
using Core.Interface.Controllers;
using Core.Interface.Models;

namespace Managers
{
    public static class PieceManager
    {
        private static List<IPieceController>[,] _piecesMatrix;
        public static List<IPieceController>[,] PiecesMatrix => _piecesMatrix;

        public static void Initialize(int mazeSize)
        {
            _piecesMatrix = new List<IPieceController>[mazeSize, mazeSize];
            for (int x = 0; x < mazeSize; x++)
                for (int y = 0; y < mazeSize; y++)
                    _piecesMatrix[x, y] = new List<IPieceController>();
        }

        public static bool AddPiece(IPieceController pieceController, (int x, int y) position)
        {
            throw new NotImplementedException();
        }

        public static bool MovePiece(IPieceController pieceController, (int x, int y) newPosition)
        {
            throw new NotImplementedException();
        }

        public static bool RemovePiece(IPieceController pieceController, (int x, int y) position)
        {
            throw new NotImplementedException();
        }

        private static bool ValidatePosition((int x, int y) position)
        {
            throw new NotImplementedException();
        }

        private static bool ValidateMove((int x, int y) position)
        {
            throw new NotImplementedException();
        }
    }
}