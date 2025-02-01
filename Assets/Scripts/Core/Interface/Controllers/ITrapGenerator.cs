using Managers;

namespace Core.Interface.Controllers
{
   public interface ITrapGenerator
   {
       int NumberOfTrapsTypes { get; }
       TrapType[,] GetNewTrapMatrix();
    }
}