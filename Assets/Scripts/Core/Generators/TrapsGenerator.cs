using Core.Interfaces;
using UnityEngine;
using Random = System.Random;

namespace Core.Generators
{
    public class TrapsGenerator : ITrapsGenerator
    {
        private int _size;
        private Random _random;
        private TrapTypes[,] _trapsMatrix;
        private int _trapChance;

        private int _numberOfTrapsTypes;
        public int NumberOfTrapsTypes => _numberOfTrapsTypes;
        public TrapsGenerator(int size, Random random, int numberOfTrapsTypes, int trapChance)
        {
            _size = size;
            _random = random;
            _trapsMatrix = new TrapTypes[size, size];
            _numberOfTrapsTypes = numberOfTrapsTypes;
            _trapChance = trapChance;
        }

        private void GenerateTrapMatrix()
        {
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    if (_random.Next(100) < _trapChance)
                    {
                        int newTrap = _random.Next(_numberOfTrapsTypes) + 1;
                        _trapsMatrix[i, j] = (TrapTypes) newTrap;
                    } 
                    else {_trapsMatrix[i, j] = 0;}
                }
            }
        }

        public TrapTypes[,] GetNewTrapMatrix()
        {
            GenerateTrapMatrix();
            return _trapsMatrix;
        }
    }
}