using System;
using System.Collections;
using System.Collections.Generic;
using Core.Interface.Models;

namespace Core.Interface.Controllers
{
    public interface ICubeController
    {
        ICubeModel Model { get; }
        
        Action<List<(int x, int y)>> OnCellsChanged { get; set; }
        
        void InitializeMaze(int size, int totalPoints); //Esto tiene que recibir los datos, y mandar a generar el laberinto en cada cara e inicializar los puntos
        void Rotate(bool horizontal, bool clockwise, int index);
        void GenerateMaze();
    }
}