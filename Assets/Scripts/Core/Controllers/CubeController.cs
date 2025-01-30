using Core.Interface.Controllers;
using Core.Interface.Models;

namespace Core.Controllers
{
    public class CubeController : ICubeController
    {
        private ICubeModel _cubeModel;
        public ICubeModel Model => _cubeModel;
        
        public void InitializeMaze()
        {
            throw new System.NotImplementedException();
        }

        public void Rotate(bool horizontal, bool clockwise, int index)
        {
            throw new System.NotImplementedException();
        }
    }
}