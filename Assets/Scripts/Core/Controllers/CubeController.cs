using System;
using Core.Interface.Controllers;
using Core.Interface.Models;
using Core.Models;
using Managers;

namespace Core.Controllers
{
    public partial class CubeController : ICubeController
    {
        public ICubeModel Model { get; private set; }
        private IMazeGenerator _mazeGenerator;
        private int _size;
        public void InitializeMaze(int size)
        {
            Model = new CubeModel(size);
            _mazeGenerator = new MazeGenerator();
            _size = size;
        }
        public void GenerateMaze()
        {
            for (int i = 0; i < 6; i++)
                Model.SetFace(i, _mazeGenerator.GenerateMaze(_size,_size, 0));
        }
    }
}