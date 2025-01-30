using Core.Interface.Models;

namespace Core.Interface.Controllers
{
    public interface ICubeController
    {
        ICubeModel Model { get; }
        
        void InitializeMaze(); //Esto tiene que recibir los datos, y mandar a generar el laberinto en cada cara e inicializar los puntos
        void Rotate(bool horizontal, bool clockwise, int index);
    }
}