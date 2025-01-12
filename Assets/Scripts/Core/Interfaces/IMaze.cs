using System.Collections.Generic;
using Core.Interfaces.Entities;

namespace Core.Interfaces
{
    public interface IMaze
    {
        int Size { get; }
        int NumberOfFaces { get; }
        IMazeFace[] MazeCube { get; }
        List<ITokenController>[,] TokensMaze { get; }
        IMazeFace GetFace(int faceIndex);
        void SetFace(int faceIndex, ICell[,] cells);
    }
}