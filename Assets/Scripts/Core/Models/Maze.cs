using System;
using System.Collections.Generic;
using Core.Interfaces;
using Core.Interfaces.Entities;

namespace Core.Models
{
    public class Maze : IMaze
    {

        private int _size;
        public int Size => _size;
        
        private int _numberOfFaces;
        public int NumberOfFaces => _numberOfFaces;
        
        private IMazeFace[] _mazeCube;
        public IMazeFace[] MazeCube => _mazeCube;
        
        private List<ITokenController>[,] _tokensMaze;
        public List<ITokenController>[,] TokensMaze => _tokensMaze;

        private SOsManager _SOsManager;

        public Maze(int size, int numberOfFaces, SOsManager sosManager)
        {
            _size = size;
            _numberOfFaces = numberOfFaces;
            _mazeCube = new IMazeFace[numberOfFaces];
            _tokensMaze = new List<ITokenController>[size, size];
            _SOsManager = sosManager;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    _tokensMaze[i, j] = new List<ITokenController>();
                }
            }

            for (int i = 0; i < numberOfFaces; i++)
            {
                _mazeCube[i] = new MazeFace(size);
            }
        }

        public IMazeFace GetFace(int faceIndex) => MazeCube[faceIndex];

        public void SetFace(int faceIndex, ICell[,] cells)
        {
            if (faceIndex < 0 || faceIndex >= NumberOfFaces)
                throw new ArgumentOutOfRangeException(nameof(faceIndex));

            _mazeCube[faceIndex] = new MazeFace(cells);
        }
    }
}